'para compilar utilizar: "dotnet publish -c Release -r win-x86" en la carpeta donde este el archivo main.vbproj y el archivo imprimir.vb juntos
'!Al agregar una libreria nueva es IMPORTANTE recordar que se necesita colocarla tambien en el archivo .vbproj dentro de <ItemGroup>

Imports System
Imports System.Drawing.Printing
Imports System.IO
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Xml

Module PrintAppV2
    ' Variables para formato de texto
    Public fontName As String = "Arial"
    Public style As FontStyle = FontStyle.Regular
    Public fontSize As Integer = 8
    Public marginTop As Integer = 0
    Public marginLeft As Integer = 0
    Public format As New StringFormat()
    Public currentFont As Font
    Public yPosition As Integer
    Public printDoc As New PrintDocument()

    Public isFormatting As Boolean = False
    ' Stack para manejar los formatos anidados
    Public formatStack As New Stack(Of TextFormat)

    Sub Main(args As String())
        Dim txtFilePath As String = args(0)
        If Not File.Exists(txtFilePath) Then
            Console.WriteLine("El archivo no existe.")
            Return
        End If
        Dim facturaContenido As String = File.ReadAllText(txtFilePath)

        Try
            FormatTxt(facturaContenido)
        Catch ex As Exception
            Dim errorMessage As String = "Error: " & ex.Message
            File.WriteAllText("error.txt", errorMessage)
        End Try
    End Sub

    Sub FormatTxt(facturaContenido As String)
        Dim TOKENSTART As String = "#@-"
        Dim TOKENEND As String = "-#@"
        Dim lines As String() = facturaContenido.Split(Environment.NewLine)


        AddHandler printDoc.PrintPage, Sub(sender, e)
                                           yPosition = marginTop
                                           For Each line As String In lines
                                               ' Manejar inicio de formato
                                               If line.Trim().StartsWith(TOKENSTART) Then
                                                   ' Guardar formato actual en el stack

                                                   SaveCurrentFormat()
                                                   ' Configurar nuevo formato
                                                   line = line.Replace(TOKENSTART, "").Replace(" ", "")
                                                   Dim array As String() = line.Split(","c)
                                                   For Each attribute As String In array
                                                       Dim keyValue As String() = attribute.Split("="c)
                                                       Dim key As String = keyValue(0)
                                                       Dim value As String = keyValue(1)
                                                       GetFormat(key, value)
                                                   Next

                                                   isFormatting = True

                                                   ' Manejar fin de formato
                                               ElseIf line.Trim().StartsWith(TOKENEND) Then

                                                   ' Restablecer formato desde el stack
                                                   RestorePreviousFormat()

                                                   ' Manejar líneas formateadas
                                               ElseIf isFormatting = True And Not line.Trim().StartsWith(TOKENEND) And Not line.StartsWith(TOKENSTART) Then
                                                   AddTextFormat(line.Trim(), e)
                                               End If
                                           Next
                                           e.HasMorePages = False
                                       End Sub
        printDoc.Print()
    End Sub

    ' Método para guardar el formato actual en el stack
    Sub SaveCurrentFormat()
        Dim currentFormat As New TextFormat(fontName, style, fontSize, marginTop, marginLeft, format.Alignment)
        formatStack.Push(currentFormat)
    End Sub

    ' Método para restablecer el formato desde el stack
    Sub RestorePreviousFormat()
        If formatStack.Count > 0 Then
            Dim previousFormat As TextFormat = formatStack.Pop()
            fontName = previousFormat.FontName
            style = previousFormat.Style
            fontSize = previousFormat.FontSize
            marginTop = previousFormat.MarginTop
            marginLeft = previousFormat.MarginLeft
            format.Alignment = previousFormat.Alignment
        Else
            isFormatting = False
        End If
    End Sub

    ' Método para configurar el formato basado en las claves y valores
    Sub GetFormat(key As String, value As String)
        Select Case key.ToLower()
            Case "fontname"
                fontName = value
            Case "style"
                style = DirectCast([Enum].Parse(GetType(FontStyle), value, True), FontStyle)
            Case "margintop"
                marginTop = Integer.Parse(value)
            Case "marginleft"
                marginLeft = Integer.Parse(value)
            Case "fontsize"
                fontSize = Integer.Parse(value)
            Case "content"
                Select Case value.ToLower()
                    Case "near"
                        format.Alignment = StringAlignment.Near
                    Case "center"
                        format.Alignment = StringAlignment.Center
                    Case "far"
                        format.Alignment = StringAlignment.Far
                End Select
        End Select
    End Sub

    ' Método para agregar texto formateado a la página
    Sub AddTextFormat(text As String, e As PrintPageEventArgs)
        currentFont = New Font(fontName, fontSize, style)
        e.Graphics.DrawString(text, currentFont, Brushes.Black, marginLeft, yPosition, format)
        yPosition += currentFont.Height + marginTop
    End Sub

    ' Clase para almacenar los formatos de texto
    Public Class TextFormat
        Public Property FontName As String
        Public Property Style As FontStyle
        Public Property FontSize As Integer
        Public Property MarginTop As Integer
        Public Property MarginLeft As Integer
        Public Property Alignment As StringAlignment

        Public Sub New(fontName As String, style As FontStyle, fontSize As Integer, marginTop As Integer, marginLeft As Integer, alignment As StringAlignment)
            Me.FontName = fontName
            Me.Style = style
            Me.FontSize = fontSize
            Me.MarginTop = marginTop
            Me.MarginLeft = marginLeft
            Me.Alignment = alignment
        End Sub
    End Class
End Module
