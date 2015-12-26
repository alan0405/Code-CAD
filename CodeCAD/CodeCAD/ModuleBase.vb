Imports AutoCAD
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Module ModuleBase

#Region "常数"
    Public Const PI As Double = Math.PI
    
#End Region

#Region "基本变量"
    Public App As AcadApplication
    Public Doc As AcadDocument
    Public Msp As AcadModelSpace
    Public Uti As AcadUtility

    Public Property UpdateDelay As Integer = 0
#End Region

#Region "窗口设置"
    Public Structure RECT
        Public left As Int32
        Public top As Int32
        Public right As Int32
        Public bottom As Int32
    End Structure
    Declare Function GetWindowRect Lib "user32" Alias "GetWindowRect" (ByVal hwnd As Integer, ByRef lpRect As RECT) As Integer
    Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As IntPtr,
                                                       ByVal hWndInsertAfter As Integer,
                                                       ByVal X As Integer, ByVal Y As Integer,
                                                       ByVal cx As Integer, ByVal cy As Integer,
                                                       ByVal wFlags As Integer) As Integer

    '窗口对其
    Public Sub SetWinPos(Optional leftName As String = "CodeCAD ")
        For Each pr As Process In Process.GetProcesses
            If pr.MainWindowTitle.Contains("CodeCAD ") Then
                SetWindowPos(pr.MainWindowHandle, 0, 0, 0, Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height, &H40)


            End If
            If pr.MainWindowTitle.Contains("AutoCAD") Then

                SetWindowPos(pr.MainWindowHandle, 0, Screen.PrimaryScreen.WorkingArea.Width / 2, 0, Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height, &H40)

            End If
        Next
    End Sub
#End Region

#Region "基础方法"

   
    '连接到AUTOCAD
    Public Function ConnectToCAD(Optional ProjectName As String = "", Optional pUpdateDelay As Integer = 0) As Boolean
        Try
            App = GetObject(, "AutoCAD.Application")
        Catch ex As Exception
            MsgBox("未能连接到AutoCAD")
            Return False
        End Try
        Try
            Doc = App.ActiveDocument
        Catch ex As Exception
            MsgBox("未能连接到AutoCAD")
            Return False
        End Try
        Msp = Doc.ModelSpace
        Uti = Doc.Utility
        If ProjectName.Length > 0 Then SetWinPos(ProjectName)
        UpdateDelay = pUpdateDelay
        Return True
    End Function
    '清除
    Public Sub Clear()
        On Error Resume Next
        Do Until Msp.Count = 0
            Msp.Item(0).Delete()
        Loop
    End Sub
    '更新
    Public Sub Update(Optional Force As Boolean = False)
        If Force = False Then
            If UpdateDelay = 0 Then Exit Sub
        End If
        App.Update()
        App.ZoomAll()
        Threading.Thread.Sleep(UpdateDelay)
    End Sub

    '角度换算360->2PI
    Public Function ToPI(angle As Double) As Double
        Dim res As Double = angle / 180
        res = res * PI
        Return res
    End Function

    '引入
    Public Function Import(fn As String, p1() As Double, Optional rotateAngle As Double = 0, Optional updown As Boolean = True) As Object

        Dim obj As AcadBlockReference = Msp.InsertBlock(p1, IO.Directory.GetCurrentDirectory + "\lib\" + fn + ".dwg", 1, 1, 1, 0)
        If rotateAngle > 0 Then
            obj.Rotate(p1, ToPI(rotateAngle))
        End If
        If updown = False Then
            obj.Rotate3D(p1, P(p1, 1, 0, 0), PI)
        End If
        Update()

        Dim objex() As Object = obj.Explode
        obj.Delete()
        Return objex(0)
    End Function
    '获得列表
    Public Function Array(ParamArray objs() As Object) As Object()
        Return objs
    End Function

    Public Function List(Of T)(ParamArray objs() As T) As List(Of T)
        Dim l As New List(Of T)
        l.AddRange(objs)
        Return l
    End Function
    '点
    Public Function P(Optional x As Double = 0,
                      Optional y As Double = 0,
                      Optional z As Double = 0) As Double()

        Dim p1(0 To 2) As Double
        p1(0) = x
        p1(1) = y
        p1(2) = z
        Return p1
    End Function

    '偏移点
    Public Function P(point() As Double, Optional x As Double = 0,
                                         Optional y As Double = 0,
                                         Optional z As Double = 0) As Double()

        Return P(point(0) + x, point(1) + y, point(2) + z)
    End Function

    '拉伸面域
    Public Function Extrude(region As AcadRegion, height As Double) As Object
        Dim so As Object = Msp.AddExtrudedSolid(region, height, 0)
        region.Delete()
        Update()
        Return so
    End Function

    '拉伸多个面域
    Public Function Extrude(region As List(Of AcadRegion), height As Double) As Object()
        Dim sos As New List(Of Object)
        For Each r As Object In region
            sos.Add(Extrude(r, height))
        Next
        Return sos.ToArray
    End Function

    '布尔与运算
    Public Function Union(ByRef obj1 As Object, ParamArray obj2() As Object) As Object
        For Each sobj As Object In obj2
            obj1.Boolean(AcBooleanType.acUnion, sobj)
            Update()
        Next
        Return obj1
    End Function

    '布尔差运算
    Public Function Subtraction(ByRef obj1 As Object, ParamArray obj2() As Object) As Object
        For Each sobj As Object In obj2
            obj1.Boolean(AcBooleanType.acSubtraction, sobj)
            Update()
        Next
        Return obj1
    End Function

    '移动一个物体
    Public Sub MoveTo(obj As Object, point() As Double)
        obj.Move(P, point)
        Update()
    End Sub

    '非负
    Public Sub NonNegative(ByRef val As Double)
        If val < 0 Then val = -val
    End Sub

    '环形阵列
    Public Function ArrayPolar(target As Object, number As Integer, angle As Double, center() As Double) As Object()
        Dim obj = target.ArrayPolar(number, ToPI(angle), center)
        Update()
        Return obj
    End Function

    '阵列
    Public Function ArrayRect(target As Object, num_x As Integer, num_y As Integer, num_z As Integer, disBetweenX As Double, disBetweenY As Double, disBetweenZ As Double) As Object()
        Dim obj = target.ArrayRectangular(num_y, num_x, num_z, disBetweenY, disBetweenX, disBetweenZ)
        Update()

        Return obj
    End Function

    '复制到p

    Public Function Copy(obj As Object) As Object
        Dim resObj As Object = obj.Copy
        MoveTo(resObj, P)
        Return resObj
    End Function

    Public Function CopyTo(obj As Object, p1() As Double) As Object
        Dim resObj As Object = obj.Copy
        MoveTo(resObj, p1)
        Return resObj
    End Function

    Public Function CopyTo(obj() As Object, p1() As Double) As Object
        Dim resObj As New List(Of Object)
        For Each o As Acad3DSolid In obj
            Dim o1 = o.Copy
            MoveTo(o1, p1)
            resObj.Add(o1)
        Next
        Return resObj.ToArray
    End Function

    '3d旋转
    Public Sub Rotate3D(obj As Acad3DSolid, basePoint() As Double, ax As RotateAX, angle As Double)
        Dim p2() As Double = basePoint
        Select Case ax
            Case RotateAX.x
                p2 = P(basePoint, 1, 0, 0)
            Case RotateAX.y
                p2 = P(basePoint, 0, 1, 0)
            Case RotateAX.z
                p2 = P(basePoint, 0, 0, 1)
        End Select
        obj.Rotate3D(basePoint, p2, ToPI(angle))
    End Sub


#End Region

#Region "基本形状"

    Public Function Line(p1() As Double, p2() As Double) As Object
        Dim obj As Object


        obj = Msp.AddLine(p1, p2)
        Update()
        Return obj
    End Function

    Public Function Circle(d As Double, p1() As Double) As Object
        Dim obj = Msp.AddCircle(p1, d / 2)
        Update()
        Return obj
    End Function

    Public Function PLine(ParamArray ps() As Object) As Object
        Dim obj As Object
        Dim a As New List(Of Double)
        For Each pn As Object In ps
            a.AddRange(pn)
        Next
        obj = Msp.Add3DPoly(a.ToArray)
        Update()
        Return obj
    End Function

    Public Function Region(ParamArray ps() As Object) As Object
        Dim obj As Object
        Dim pl As Object = PLine(ps)
        obj = Msp.AddRegion(pl)
        Update()
        Return obj
    End Function

    Public Function Region(ParamArray entitys() As AcadEntity) As Object
        Dim obj As Object
        obj = Msp.AddRegion(entitys)
        Update()
        Return obj
    End Function

    '圆柱体
    Public Function Cylinder(diameter As Double, height As Double, basePoint() As Double) As Object

        Dim point() As Double = P(basePoint, 0, 0, height / 2)

        NonNegative(height)

        Dim cy As Object = Msp.AddCylinder(point, diameter / 2, height)
        Update()
        Return cy
    End Function

    '方块
    Public Function Cube(width_x As Double, length_y As Double, height_z As Double, basePoint() As Double) As Object

        Dim point() As Double = P(basePoint, width_x / 2, 0, height_z / 2)

        NonNegative(width_x)
        NonNegative(length_y)
        NonNegative(height_z)

        Dim cu As Object = Msp.AddBox(point, width_x, length_y, height_z)
        Update()
        Return cu
    End Function

    '楔形
    Public Function Wedge(width As Double, length As Double, height As Double, basepoint() As Double) As Object

        Dim point() As Double = P(basepoint, width / 2, 0, height / 2)
        NonNegative(width)
        NonNegative(length)
        NonNegative(height)

        Dim we As Object = Msp.AddWedge(point, width, length, height)
        Update()
        Return we
    End Function

    '拉伸
    Public Function ExtrudedSolid(region As Object, height As Double) As Object
        Dim obj As Acad3DSolid
        obj = Msp.AddExtrudedSolid(region, height, 0)
        Update()
        Return obj
    End Function

    Public Function RevolvedSolid(region As Object, p1 As Double(), p2 As Double(), angle As Double) As Object
        Dim obj As Object
        obj = Msp.AddRevolvedSolid(region, p1, p2, ToPI(angle))
        region.Delete()
        Update()
        Return obj
    End Function

    Public Function ExtrudedSolid(lines() As Object, height As Double) As Object
        Dim region As Object = Msp.AddRegion(lines)
        Return ExtrudedSolid(region, height)
    End Function

#End Region


End Module

Public Enum RotateAX
        x
        y
        z
    End Enum

Public Class PList
    Inherits List(Of Double())

    Public Sub RelativeToLast(Optional x As Double = 0, Optional y As Double = 0, Optional z As Double = 0)
        If Count = 0 Then Add(P)
        Add(P(Item(Count - 1), x, y, z))
    End Sub

End Class