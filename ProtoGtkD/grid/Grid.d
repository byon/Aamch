import gtk.CellRendererText;
import gtk.Container;
import gtk.ListStore;
import gtk.TreeIter;
import gtk.TreeModelIF;
import gtk.TreePath;
import gtk.TreeSelection;
import gtk.TreeView;
import gtk.TreeViewColumn;

import std.conv;

class Grid
{
    interface SelectionListener
    {
        void Changed(string columns);
    }

    this(SelectionListener listener)
    {
        selectionListener = listener;
        tree = new TreeView;
        GType[20] types;
        types[] = GType.STRING;
        store = new ListStore(types);
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

    void AddRow( )
    {
        auto iterator = store.createIter( );
        uint row = rows++;
        store.setValue(iterator, 0, to!string(row + 1));
        foreach (uint column; 1..columns)
        {
            store.setValue(iterator, column, "foo" ~ to!string(row));
        }
    }

    uint GetColumnCount( )
    {
        return columns;
    }

    void AddIntoContainer(Container container)
    {
        container.add(tree);
    }

    private void SelectionChanged(TreeSelection selection)
    {
        TreeModelIF model;
        auto selected = selection.getSelectedRows(model);

        string result;
        foreach(TreePath path; selected)
        {
            result ~= path.toString( );
        }

        selectionListener.Changed(result);
    }

    private ListStore store;
    private TreeView tree;
    private uint columns;
    private uint rows;
    private SelectionListener selectionListener;
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
