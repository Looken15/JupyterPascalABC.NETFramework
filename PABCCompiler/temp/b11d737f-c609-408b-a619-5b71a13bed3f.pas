uses RedirectIOMode1;
begin
    var x := ReadInteger('Введите x:');
    Println('x = ' + x);
    var y := ReadInteger('Введите y:');
    Println('y = ' + y);
    Println('x * y = ' + (x * y));
end.