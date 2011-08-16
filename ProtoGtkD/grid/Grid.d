import gtk.ListStore;
import gtk.TreeIter;
import gtk.TreeView;
import gtk.TreeViewColumn;
import gtk.CellRendererText;

import std.conv;
/// @todo sort

class Gridd
{
    ListStore store;
    TreeView tree;
    uint columns;

    this( )
    {
        tree = new TreeView;
        store = new ListStore([]);
        tree.setModel(store);
    }

    void AddColumn(string name)
    {
        auto column = new TreeViewColumn;
        column.setTitle = name;
        tree.appendColumn(column);

        auto text = new CellRendererText;
        column.packStart(text, 0);

        uint index = columns++;
        column.addAttribute(text, "text", index);
        column.setSortColumnId(index);

        store.setSortFunc(index, &DoSort, cast(void*)store, null);
    }

    void AddRow(uint row)
    {
        auto iterator = store.createIter( );
        store.setValue(iterator, 0, to!string(row + 1));
        foreach (uint column; 1..columns)
        {
            store.setValue(iterator, column, "foo" ~ to!string(row));
        }
    }

}

private extern (C) int DoSort(GtkTreeModel* model, GtkTreeIter* first,
                              GtkTreeIter* second, void* userData)
{
    auto store = cast(ListStore)userData;

    int sortColumn;
    GtkSortType order;
    store.getSortColumnId(sortColumn, order);

    return std.string.cmp(ValueAsString(store, first, sortColumn),
                          ValueAsString(store, second, sortColumn));
}

private string ValueAsString(ListStore store, GtkTreeIter* iterator, int column)
{
    return store.getValueString(new TreeIter(iterator), column);
}
