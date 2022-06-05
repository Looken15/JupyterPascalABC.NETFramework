uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,300);
    Brush.Color := Colors.Red;
    Pen.Color := Colors.Blue;
    Pen.Width := 2;
    
    Rectangle(10,10,200,100);
    DrawRectangle(220,10,200,100);
    FillRectangle(430, 10,200,100);
    
    Rectangle(10,120,200,100,Colors.Yellow);
    DrawRectangle(220,120,200,100,Colors.Yellow);
    FillRectangle(430, 120,200,100,Colors.Yellow);
    
    NeedButtons(true);
end.