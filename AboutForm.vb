Imports System.Drawing
Imports System.Windows.Forms

Public Class AboutForm
    Inherits Form

    Private labelTitle As Label
    Private textBoxExplanation As TextBox
    Private buttonClose As Button

    Public Sub New()
        ' Initialize the form components
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.labelTitle = New Label()
        Me.textBoxExplanation = New TextBox()
        Me.buttonClose = New Button()

        ' Configure the form
        Me.Text = "PrintApp"
        Me.Size = New Size(600, 500)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' Configure the title
        Me.labelTitle.Text = "Tutorial: How to Use PrintApp"
        Me.labelTitle.Font = New Font("Arial", 14, FontStyle.Bold)
        Me.labelTitle.Location = New Point(20, 20)
        Me.labelTitle.Size = New Size(560, 30)
        Me.Controls.Add(Me.labelTitle)

        ' Configure the text area for the explanation
        Me.textBoxExplanation.Multiline = True
        Me.textBoxExplanation.ScrollBars = ScrollBars.Vertical
        Me.textBoxExplanation.Text = "### Purpose of the Program" & vbCrLf &
            "PrintApp is a tool that allows you to print text with specified formatting from a text file." & vbCrLf & vbCrLf &
            "### How It Works" & vbCrLf &
            "1. **Input File**: The program takes a text file as input. This file should contain text and formatting tokens to adjust the appearance of the printed text." & vbCrLf & vbCrLf &
            "2. **Formatting Tokens**: Tokens are used to apply formatting to the text." & vbCrLf &
            "- **`#@-`**: Indicates the start of a formatting section. For example," & vbCrLf &
            "`#@-fontname=Arial,style=Bold,fontsize=12,margintop=10,marginleft=15,content=center`" & vbCrLf &
            "- **`-#@`**: Indicates the end of a formatting section." & vbCrLf & vbCrLf &
            "### Example Input File" & vbCrLf &
            "````" & vbCrLf &
            "#@-fontname=Arial,style=Bold,fontsize=12,margintop=10,marginleft=15,content=center" & vbCrLf &
            "   This is bold, centered text with formatting." & vbCrLf &
            "-#@" & vbCrLf &
            "````" & vbCrLf & vbCrLf &
            "### Using the Program" & vbCrLf &
            "To run the program, use the following command in the command line: " & vbCrLf &
            "`dotnet publish -c Release -r win-x86`." & vbCrLf &
            "This compiles and publishes the application for execution on Windows x86 systems." & vbCrLf & vbCrLf &
            "author: hemeh" & vbCrLf &
            "GitHub: @hemeh12"

        Me.textBoxExplanation.Location = New Point(20, 60)
        Me.textBoxExplanation.Size = New Size(550, 350)
        Me.textBoxExplanation.Font = New Font("Arial", 10)
        Me.textBoxExplanation.ReadOnly = True
        Me.Controls.Add(Me.textBoxExplanation)

        ' Configure the close button
        Me.buttonClose.Text = "Close"
        Me.buttonClose.Location = New Point(495, 420)
        Me.buttonClose.Size = New Size(75, 30)
        AddHandler Me.buttonClose.Click, AddressOf Me.ButtonClose_Click
        Me.Controls.Add(Me.buttonClose)
    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
End Class
