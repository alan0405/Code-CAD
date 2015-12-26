Module Module3

#Region "   基本参数列表"

    '基本参数
    '马达直径
    Private Const MoDiameter As Double = 37
    '马达高度
    Private Const MoHeight As Double = 90
    '马达轴台直径
    Private Const MoOutDiameter As Double = 12
    '马达轴台高度
    Private Const MoOutHeight As Double = 6
    '马达轴偏移量
    Private Const MoAxOffset As Double = 7

    '轴承直径
    Private Const BearingDiameter As Double = 10

    '皮带轮直径
    Private Const PuDiameter As Double = 22
    '皮带轮高度
    Private Const PuHeight As Double = 14.5

    Private Const PuShellHeight As Double = PuHeight + 3

    '轴距
    Private Const AxDistance As Double = 99

    '壳体厚度
    Private Const ShellThickness As Double = 2

    '电线直径
    Private Const LineDiameter As Double = 4.5

#End Region

    Public Sub GateWay()
        'DrawMoShell()
        ShellCover()
        PuShell()
    End Sub


    Private Sub DrawMoShell()
        '基点
        Dim basePoint = P()
        '                           马达直径        偏移  壳体厚度
        Dim baseDiameter As Double = MoDiameter + 1 + ShellThickness * 2
        '整体高度
        Dim baseHight As Double = MoHeight + ShellThickness
        '外部圆柱体
        Dim body = Cylinder(baseDiameter, -baseHight, basePoint)
        '内部圆柱体
        Dim cyInside = Cylinder(baseDiameter - ShellThickness * 2, -baseHight + ShellThickness, basePoint)
        '紧固件
        Dim fastener As Object = Import("NutSet", P(basePoint, baseDiameter / 2, 0, 0), 0, False)
        '阵列紧固件
        Dim fasteners = ArrayPolar(fastener, 5, 360, P)
        '底部孔
        Dim holeCy = Cylinder(LineDiameter, ShellThickness, P(basePoint, 0, 0, -baseHight))
        '组合
        Union(body, fastener)
        Union(body, fasteners)
        '布尔减
        Subtraction(body, cyInside)
        Subtraction(body, holeCy)

    End Sub

    Private Sub ShellCover()
        Dim basePoint = P()
        '
        Dim baseDiameter As Double = MoDiameter + 1 + ShellThickness * 2 + 14
        Dim outDiameter As Double = PuDiameter + 1 + 7.5 + ShellThickness * 2
        '电机端
        Dim moCy = Cylinder(baseDiameter, MoOutHeight, basePoint)
        '皮带轮端
        Dim puCy = Cylinder(outDiameter, MoOutHeight - 2, P(basePoint, MoAxOffset + AxDistance, 0, 2))
        '中间矩形
        Dim body = Cube(MoAxOffset + AxDistance, outDiameter, MoOutHeight - 2, P(basePoint, 0, 0, 2))

        Union(body, moCy, puCy)

        '电机外壳紧固孔
        Dim fCy = Cylinder(3.3, MoOutHeight, P(basePoint, baseDiameter / 2 - 7 + 3.4, 0, 0))
        Dim fCys = ArrayPolar(fCy, 5, 360, basePoint)

        Subtraction(body, fCy)
        Subtraction(body, fCys)

        '电机紧固孔

        Dim mofcy = Cylinder(3.3, 6, P(basePoint, 0, 31 / 2, 0))

        Dim mofcy1 = Cylinder(5.3, 2, P(basePoint, 0, 31 / 2, 4))
        Union(mofcy, mofcy1)
        Dim mofcys = ArrayPolar(mofcy, 6, 360, basePoint)

        Subtraction(body, mofcy)
        Subtraction(body, mofcys)

        '电机轴孔

        Dim moOutCy = Cylinder(MoOutDiameter + 0.3, 6, P(basePoint, MoAxOffset, 0, 0))

        Subtraction(body, moOutCy)

        '皮带轮轴孔

        Dim puOutCy = Cylinder(BearingDiameter - 2, 1, P(basePoint, MoAxOffset + AxDistance, 0, 2))
        Dim puOutCy1 = Cylinder(BearingDiameter + 0.5, 3, P(basePoint, MoAxOffset + AxDistance, 0, 2 + 1))

        Subtraction(body, puOutCy)
        Subtraction(body, puOutCy1)

        '外壳紧固孔
        '皮带轮侧紧固件
        Dim m2f = Cylinder(2.2, 4, P(basePoint, MoAxOffset + AxDistance, outDiameter / 2 - 3, 2))
        Dim m2fa = Cylinder(4, 1.5, P(basePoint, MoAxOffset + AxDistance, outDiameter / 2 - 3, 2))

        Union(m2f, m2fa)
        '阵列

        Dim m2fs = ArrayPolar(m2f, 5, -180, P(basePoint, MoAxOffset + AxDistance, 0, 2))

        Dim m2f1 = CopyTo(m2f, P(-AxDistance / 2, 0, 0))
        Dim m2f2 = CopyTo(m2f1, P(0, -outDiameter + 6, 0))
        'Dim m2f3 = CopyTo(m2f2, P(8))
        'Dim m2f4 = CopyTo(m2f3, P(8))
        Subtraction(body, m2f, m2f1, m2f2)
        Subtraction(body, m2fs)


    End Sub

    Private Sub PuShell()
        Dim basePoint = P(0, 0, 6)
        '
        Dim baseDiameter As Double = MoDiameter + 1 + ShellThickness * 2
        Dim outDiameter As Double = PuDiameter + 1 + 7.5 + ShellThickness * 2



        '电机端
        Dim moCy = Cylinder(baseDiameter, PuShellHeight + ShellThickness, basePoint)
        '皮带轮端
        Dim puCy = Cylinder(outDiameter, PuShellHeight + ShellThickness, P(basePoint, MoAxOffset + AxDistance, 0, 0))
        '中间矩形
        Dim body = Cube(MoAxOffset + AxDistance, outDiameter, PuShellHeight + ShellThickness, basePoint)

        Union(body, moCy, puCy)
        '电机端
        Dim moCy1 = Cylinder(baseDiameter - ShellThickness * 2, PuShellHeight, basePoint)
        '皮带轮端
        Dim puCy1 = Cylinder(outDiameter - ShellThickness * 2, PuShellHeight, P(basePoint, MoAxOffset + AxDistance, 0, 0))
        '中间矩形
        Dim body1 = Cube(MoAxOffset + AxDistance, outDiameter - ShellThickness * 2, PuShellHeight, basePoint)

        '紧固件
        Dim fastener As Object = Import("NutSet", P(basePoint, baseDiameter / 2, 0, 0), 0)

        '阵列紧固件
        Dim fasteners = ArrayPolar(fastener, 5, 360, P)

        Union(body, fastener)
        Union(body, fasteners)

        Subtraction(body, moCy1, puCy1, body1)

        '皮带轮侧紧固件
        Dim m2f = Import("m2fastener", P(basePoint, MoAxOffset + AxDistance, outDiameter / 2 - 3, 0), 180 * 3 / 2, False)
        '阵列

        Dim m2fs = ArrayPolar(m2f, 5, -180, P(basePoint, MoAxOffset + AxDistance, 0, 0))

        Dim m2f1 = Import("m2fastener", P(basePoint, MoAxOffset + AxDistance / 2, outDiameter / 2 - 3, 0), 180 * 3 / 2, False)
        Dim m2f2 = Import("m2fastener", P(basePoint, MoAxOffset + AxDistance / 2, -outDiameter / 2 + 3, 0), 180 / 2, False)

        'Dim m2f3 = CopyTo(m2f2, P(8))
        'Dim m2f4 = CopyTo(m2f3, P(8))
        Union(body, m2f, m2f1, m2f2)
        Union(body, m2fs)

        '轴承座

        Dim bset = Cylinder(BearingDiameter + 6 + 0.3, 2, P(basePoint, MoAxOffset + AxDistance, 0, PuShellHeight - 2))
        Dim bset1 = Cylinder(BearingDiameter + 0.3, 3, P(basePoint, MoAxOffset + AxDistance, 0, PuShellHeight - 2))

        Union(body, bset)
        Subtraction(body, bset1)

    End Sub

End Module
