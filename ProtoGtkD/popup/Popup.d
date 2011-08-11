import gtk.MainWindow;
import gtk.Button;
import gtk.MessageDialog;
import gtk.Main;

class PopupMessage : MainWindow
{
	this()
	{
		super("Popup Message - a window name");
		setDefaultSize(350, 300);
		add(new Button("Message", &popupMsg));
		showAll();
	}
	void popupMsg(Button button)
	{
		MessageDialog d = new MessageDialog(this, GtkDialogFlags.MODAL,
                                            MessageType.INFO, ButtonsType.OK,
                                            "This is a popup message!");
		d.run();
		d.destroy();
	}
}

void main(string[] args)
{
	Main.init(args);
	new PopupMessage();
	Main.run();
}
