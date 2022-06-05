uses RedirectIOMode1;
uses GraphJupyter;
begin
    WindowSize(700,300);
    for var i := 1 to 800 do
    begin
        SetPixel(Random(0,700),Random(0,300),Colors.Red);
        SetPixel(Random(0,700),Random(0,300),Colors.Blue);
    end;
    NeedButtons(true);
end.