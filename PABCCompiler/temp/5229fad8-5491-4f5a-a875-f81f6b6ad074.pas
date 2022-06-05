uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,300);
    Pen.Color := Colors.Red;
    Pen.Width := 1;
    Brush.Color := Colors.Green;
    Sector(100,100,50,0,90);
    DrawSector(200,100,50,0,90);
    FillSector(300,100,50,0,90);
    Sector(100,200,50,0,90,Colors.Purple);
    DrawSector(200,200,50,0,90,Colors.Purple);
    FillSector(300,200,50,0,90,Colors.Purple);
end.