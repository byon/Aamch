import gtk.Button;
import gtk.CellRendererText;
import gtk.HBox;
import gtk.Label;
import gtk.ListStore;
import gtk.Main;
import gtk.MainWindow;
import gtk.ScrolledWindow;
import gtk.Table;
import gtk.TreeIter;
import gtk.TreeModelIF;
import gtk.TreePath;
import gtk.TreeSelection;
import gtk.TreeView;
import gtk.TreeViewColumn;
import std.conv;

const auto COLUMNS = 3;

void main(string[] args)
{
	Main.init(args);
	new GridWindow( );
	Main.run( );
}

class GridWindow : MainWindow
{
    ListStore store;
    TreeView tree;
    Label selectedRow;
    uint rows;
    uint columns;

	this( )
	{
		super("Grid prototype");
        auto table = new Table(2, 1, false);
        add(table);
        auto box = new HBox(false, 2);
        box.add(new Button("Add new row", &OnAddNewRow));
        box.add(new Button("Add new column", &OnAddNewColumn));
        selectedRow = new Label("No row is selected");
        box.add(selectedRow);

        const AttachOptions EXPAND = AttachOptions.EXPAND | AttachOptions.FILL;
        table.attach(box, 0, 1, 0, 1, EXPAND, AttachOptions.SHRINK, 0, 0);

        auto scrolled = new ScrolledWindow(null, null);
        table.attachDefaults(scrolled, 0, 1, 1, 2);

        tree = new TreeView;
        scrolled.add(tree);

        GType[COLUMNS] types;
        types[] = GType.STRING;

        store = new ListStore(types);

        foreach (uint column; 0..COLUMNS)
        {
            AddColumn(tree, store, "column" ~ to!string(column), column);
        }

        columns = COLUMNS;

        foreach (uint row; 0..5)
        {
            AddRow(store, rows++);
        }

		setDefaultSize(400, 300);

        tree.setModel(store);

        auto selection = tree.getSelection( );
        selection.addOnChanged(&SelectionChanged);
        selection.setMode(SelectionMode.MULTIPLE);

		showAll( );
	}

    void OnAddNewRow(Button button)
    {
        AddRow(store, rows++);
    }

    void OnAddNewColumn(Button button)
    {
        // Apparently this does not really work as an idea. The "model" (in
        // practise the ListStore) does not support addition of new columns
        // after rows have been added. We'll need to create the store again
        // and add a new store into it.
        AddColumn(tree, store, "column" ~ to!string(columns), columns++);
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

        selectedRow.setText(value);
    }
}

extern (C) int Sort(GtkTreeModel* model, GtkTreeIter* first,
                    GtkTreeIter* second, void* userData)
{
    auto store = cast(ListStore)userData;

    int sortColumn;
    GtkSortType order;
    store.getSortColumnId(sortColumn, order);

    return std.string.cmp(ValueAsString(store, first, sortColumn),
                          ValueAsString(store, second, sortColumn));
}

string ValueAsString(ListStore store, GtkTreeIter* iterator, int column)
{
    return store.getValueString(new TreeIter(iterator), column);
}

void AddColumn(TreeView tree, ListStore store, string name, uint index)
{
    auto column = new TreeViewColumn;
    column.setTitle = name;
    tree.appendColumn(column);

    auto text = new CellRendererText;
    column.packStart(text, 0);
    column.addAttribute(text, "text", index);
    column.setSortColumnId(index);

    store.setSortFunc(index, &Sort, cast(void*)store, null);
}

void AddRow(ListStore store, uint row)
{
    auto iterator = store.createIter( );
    store.setValue(iterator, 0, to!string(row + 1));
    foreach (uint column; 1..COLUMNS)
    {
        store.setValue(iterator, column, "foo" ~ to!string(row));
    }
}
