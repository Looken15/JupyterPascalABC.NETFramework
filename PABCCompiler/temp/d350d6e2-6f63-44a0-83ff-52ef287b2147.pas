uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,300);
    Brush.Color := Colors.Red;
    Pen.Color := Colors.Blue;
    Pen.Width := 2;
    
    Ellipse(110,60,100,50);
    DrawEllipse(320,60,100,50);
    FillEllipse(530,60,100,50);
    
    Ellipse(110,170,100,50,Colors.Green);
    DrawEllipse(320,170,100,50,Colors.Green);
    FillEllipse(530,170,100,50,Colors.Green);
    
    NeedButtons(true);
end.