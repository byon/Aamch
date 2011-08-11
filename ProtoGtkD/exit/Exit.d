import gtk.MainWindow;
import gtk.Button;
import gtk.Main;

class ExitButton : MainWindow
{
	this()
	{
		super("Exit Button");
		setDefaultSize(150, 30);
		Button exitbtn = new Button();
		exitbtn.setLabel("Exit");
		exitbtn.addOnClicked(&exitProg);
		add(exitbtn);
		showAll();
	}
	void exitProg(Button button)
	{
		Main.exit(0);
	}
}

void main(string[] args)
{
	Main.init(args);
	new ExitButton();
	Main.run();
}
