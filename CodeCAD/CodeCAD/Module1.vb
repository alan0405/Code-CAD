Imports CodeCAD
Module Module1

    Sub Main()
        If Not ConnectToCAD("CodeCAD", 0) Then Exit Sub
        'Draw something down here...
        Clear()
        Module4.DrawBtnShell()

        Update(True)
        App.ZoomAll()
    End Sub

    Sub DrillSet()
        Dim a = Cube(30, 5, 5, P)
        Subtraction(a,
        Cylinder(1.2, 5, P(5, 0, 0)),
        Cylinder(1.8, 5, P(10, 0, 0)),
        Cylinder(1.8, 5, P(15, 0, 0)),
        Cylinder(2.8, 5, P(20, 0, 0)),
        Cylinder(2.8, 5, P(25, 0, 0)))

    End Sub

    Sub DrawFasten()

        Dim a = Cube(100, 16, 16, P)
        Dim b = Cylinder(49, 16, P(28.5, 0, 0))
        Dim b1 = Cylinder(43, 16, P(28.5, 0, 0))

        Dim c = Cylinder(3.3, 16, P(2 + 3.3 / 2, 0, 0))
        Dim c1 = Cylinder(6.45, 2, P(2 + 3.3 / 2, 0, 0))
        Dim c2 = CopyTo(c1, P(0, 0, 16))
        Union(c, c1, c2)

        Dim f = CopyTo(c, P(92.5, 0, 0))
        Rotate3D(c, P(2 + 3.3 / 2, 0, 8), RotateAX.x, PI / 2)

        Dim d = Cube(15, 5, 20, P)

        Dim e = Cylinder(25 + 6, 16, P(100 - 20.5, 7.5, 0))
        Dim e1 = Cylinder(25, 16, P(100 - 20.5, 7.5, 0))
        Rotate3D(e, P(0, 0, 8), RotateAX.x, PI / 2)
        Rotate3D(e1, P(0, 0, 8), RotateAX.x, PI / 2)
        Dim g = CopyTo(d, P(100 - 15, 0, 0))
        Rotate3D(g, P(0, 0, 10), RotateAX.x, PI / 2)
        Union(a, b, e)
        Subtraction(a, b1, c, d, e1, f, g)
    End Sub

    Sub DrawLines()

        Dim l As New PList
        l.RelativeToLast(z:=30)
        l.RelativeToLast(10)
        l.RelativeToLast(2, 0, -2)
        l.Add(P(12))
        l.Add(P)

        RevolvedSolid(Region(PLine(l.ToArray))(0), P, P(0, 0, 1), PI * 2)

    End Sub

    Sub DrawAxOfH()

    End Sub

    Sub DrawAxOfSp()
        Dim a = Cylinder(6, 21, P)

        Dim b = Cylinder(8, 3, P(0, 0, 21))

        Dim c = Cylinder(5, 18, P(0, 0, 24))

        Dim d = Cube(5, 5, 18, P(4.5 / 2, 0, 24))

        Dim e = ArrayPolar(d, 6, PI * 2, P)

        Dim f = Cube(10, 10, 18, P(2, 0, 0))

        Dim g = Cylinder(2.8, -15, P(0, 0, 42))

        Union(a, b, c)
        Subtraction(a, d)
        Subtraction(a, e)
        Subtraction(a, f, g)

    End Sub


    Sub Drawitem()

        Dim neijing = 6
        Dim waijing = 10

        Dim a = Cylinder(neijing, 3, P)
        Dim b = Cylinder(neijing + 1, 3, P)

        Subtraction(b, a)

        Dim c = Cylinder(waijing - 1, 3, P)
        Dim d = Cylinder(waijing, 3, P)

        Subtraction(d, c)

        Dim e = Cylinder(1, 2, P(4, 0, 0.5))

        Dim e1 = ArrayPolar(e, 12, PI * 2, P)

        Dim f = Cylinder(waijing - 1.5, 3, P)

        Dim g = Cylinder(neijing + 1.5, 3, P)

        Subtraction(f, g)

        Subtraction(f, CopyTo(e, P))
        Subtraction(f, CopyTo(e1, P))

    End Sub
End Module
