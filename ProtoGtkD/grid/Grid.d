import gtk.Button;
import gtk.CellRendererText;
import gtk.HBox;
import gtk.Label;
import gtk.ListStore;
import gtk.Main;
import gtk.MainWindow;
import gtk.ScrolledWindow;
import gtk.Table;
import gtk.TreeModelIF;
import gtk.TreePath;
import gtk.TreeSelection;
import gtk.TreeView;
import gtk.TreeViewColumn;
import std.conv;

const auto COLUMNS = 20;

void main(string[] args)
{
	Main.init(args);
	new Grid( );
	Main.run( );
}

class Grid : MainWindow
{
    ListStore store;
    Label selectedRow;
    uint rows;

	this( )
	{
		super("Grid prototype");
        auto table = new Table(2, 1, false);
        add(table);
        auto box = new HBox(false, 2);
        box.add(new Button("Add new row", &ButtonClicked));
        selectedRow = new Label("No row is selected");
        box.add(selectedRow);

        const AttachOptions EXPAND = AttachOptions.EXPAND | AttachOptions.FILL;
        table.attach(box, 0, 1, 0, 1, EXPAND, AttachOptions.SHRINK, 0, 0);

        auto scrolled = new ScrolledWindow(null, null);
        table.attachDefaults(scrolled, 0, 1, 1, 2);

        auto tree = new TreeView;
        scrolled.add(tree);

        foreach (uint column; 0..COLUMNS)
        {
            AddColumn(tree, "column" ~ to!string(column), column);
        }

        GType[COLUMNS] types;
        types[] = GType.STRING;

        store = new ListStore(types);

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

    void ButtonClicked(Button button)
    {
        AddRow(store, rows++);
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

void AddColumn(TreeView tree, string name, uint index)
{
    auto column = new TreeViewColumn;
    column.setTitle = name;
    tree.appendColumn(column);

    auto text = new CellRendererText;
    column.packStart(text, 0);
    column.addAttribute(text, "text", index);
}

void AddRow(ListStore store, uint row)
{
    auto iterator = store.createIter( );
    store.setValue(iterator, 0, to!string(row + 1));
    foreach (uint column; 1..COLUMNS)
    {
        store.setValue(iterator, column, "foo");
    }
}
