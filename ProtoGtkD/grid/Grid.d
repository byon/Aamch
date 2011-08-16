import gtk.CellRendererText;
import gtk.ListStore;
import gtk.TreeIter;
import gtk.TreeModelIF;
import gtk.TreePath;
import gtk.TreeSelection;
import gtk.TreeView;
import gtk.TreeViewColumn;

import std.conv;

class Gridd
{
    this( )
    {
        tree = new TreeView;
        store = new ListStore([]);
        tree.setModel(store);

        auto selection = tree.getSelection( );
        selection.addOnChanged(&SelectionChanged);
        selection.setMode(SelectionMode.MULTIPLE);
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

    void SelectionChanged(TreeSelection selection)
    {
        TreeModelIF model;
        auto selected = selection.getSelectedRows(model);

        string value;
        foreach(TreePath path; selected)
        {
            value ~= path.toString( );
        }
    }

    alias tree this; /// @todo see later, if this works

    private ListStore store;
    private TreeView tree;
    private uint columns;
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
