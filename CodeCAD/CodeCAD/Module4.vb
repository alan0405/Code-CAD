Module Module4

    Sub DrawBtnShell()
        Dim a = Cube(33, 22, 11, P)
        Dim a1 = Cube(29, 18, 11, P(2, 0, 2))
        Subtraction(a, a1)
        Dim a2 = Cube(2, 18, 9, P(20, 0, 2))
        Union(a, a2)

        Dim b = Cylinder(16, 10, P(z:=11))
        Rotate3D(b, P(z:=11), RotateAX.y, 90)
        Subtraction(a, b)
        Dim b1 = Cylinder(15.5, 15, P(10, 0, 11))
        Rotate3D(b1, P(10, 0, 11), RotateAX.y, 90)
        Subtraction(a, b1)

        Dim c = Cylinder(4, 50, P(27.5, 11, 11))
        Rotate3D(c, P(27.5, 11, 11), RotateAX.x, 90)
        Subtraction(a, c)

        Dim d = Cube(2, 13, 3, P(5, 0, 0))
        Dim d1 = CopyTo(d, P(23))
        Subtraction(a, d, d1)

        Dim d2 = Cube(25, 13, 10, P(5, 0, 2))
        Subtraction(a, d2)

        Dim e = Cylinder(3, 11, P(1, 10, 0))
        Dim es = ArrayRect(e, 2, 2, 1, 31, -20, 0)

        Dim e1 = Cylinder(2, 10, P(1, 10, 1))
        Dim e1s = ArrayRect(e1, 2, 2, 1, 31, -20, 0)

        Union(a, e)
        Union(a, es)

        Subtraction(a, e1)
        Subtraction(a, e1s)

        Dim pf = P(0, 40)

        Dim f = CopyTo(a, pf)

        Dim f1 = Cylinder(3, 8, P(pf, 1, 10, 0))

        Dim f1s = ArrayRect(f1, 2, 2, 1, 31, -20, 0)

        Subtraction(f, f1)
        Subtraction(f, f1s)
    End Sub
End Module
