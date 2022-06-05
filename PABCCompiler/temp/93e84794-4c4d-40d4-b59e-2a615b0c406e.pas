uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,300);
    Pen.Color := Colors.Blue;
    Pen.Width := 1;
    Arc(100,100,50,0,270,Colors.Red);
    Arc(200,100,50,-180,0);
    Arc(300,100,50,-90,180,Colors.Red);
    Arc(100,200,50,0,90);
    Arc(300,200,50,90,180);
    Arc(200,200,50,-180,0,Colors.Red);
    needbuttons(true);
end.