uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,300);
    Pen.Color := Colors.Red;
    Pen.Width := 2;
    Line(10,10,100,100);
    Line(20,10,110,100,Colors.Blue);
    
    var arr1:= Arr((new Point(50,10),new Point(150,100)),
                 (new Point(60,10),new Point(160,100)));
    Lines(arr1,Colors.Purple);
    
    var arr:= Arr(new Point(200,10),new Point(300,100),
                  new Point(400,10),new Point(500,100));
    PolyLine(arr,Colors.Pink);
end.