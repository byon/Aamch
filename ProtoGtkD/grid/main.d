import gtk.Button;
import gtk.Container;
import gtk.HBox;
import gtk.Label;
import gtk.Main;
import gtk.MainWindow;
import gtk.ScrolledWindow;
import gtk.Table;

import std.conv;
import std.stdio;

import grid.Grid;

const auto COLUMNS = 3;

void main(string[] args)
{
	Main.init(args);
	new GridWindow( );
	Main.run( );
}

class GridWindow : MainWindow
{
	this( )
	{
		super("Grid prototype");
        auto table = new Table(3, 1, false);
        add(table);
        auto box = new HBox(false, 2);
        box.add(new Button("Add new row", &OnAddNewRow));
        box.add(new Button("Add new column", &OnAddNewColumn));

        Label label = new Label("No row selected");
        box.add(label);

        selectionHandler = new SelectionHandler(label);

        const AttachOptions EXPAND = AttachOptions.EXPAND | AttachOptions.FILL;
        table.attach(box, 0, 1, 0, 1, EXPAND, AttachOptions.SHRINK, 0, 0);

        topGrid = AddScrollableGrid(table, 1);
        bottomGrid = AddScrollableGrid(table, 2);

		setDefaultSize(400, 300);

		showAll( );
	}

    Grid AddScrollableGrid(Table table, uint row)
    {
        auto scrolled = new ScrolledWindow(null, null);
        table.attachDefaults(scrolled, 0, 1, row, row +1);

        return AddGrid(scrolled);
    }

    Grid AddGrid(Container container)
    {
        auto result = new Grid(selectionHandler);

        foreach (uint column; 0..COLUMNS)
        {
            result.AddColumn("column" ~ to!string(column));
        }

        foreach (uint row; 0..5)
        {
            result.AddRow( );
        }

        result.AddIntoContainer(container);

        return result;
    }

    void OnAddNewRow(Button button)
    {
        topGrid.AddRow( );
    }

    void OnAddNewColumn(Button button)
    {
        topGrid.AddColumn("column" ~ to!string(topGrid.GetColumnCount( )));
    }

    private class SelectionHandler : Grid.SelectionListener
    {
        this(Label selectedRow)
        {
            this.selectedRow = selectedRow;
        }

        void Changed(string columns)
        {
            selectedRow.setText(columns);
        }

        private Label selectedRow;
    }

    Grid topGrid;
    Grid bottomGrid;
    SelectionHandler selectionHandler;
}
