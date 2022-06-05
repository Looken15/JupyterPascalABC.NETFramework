uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,230);
    Brush.Color := Colors.Red;
    Pen.Color := Colors.Blue;
    Pen.Width := 1;
    Circle(60,60,50);
    DrawCircle(170,60,50);
    FillCircle(280,60,50);
    Circle(60,170,50,Colors.Green);
    DrawCircle(170,170,50,Colors.Green);
    FillCircle(280,170,50,Colors.Green);
    NeedButtons(true);
end.