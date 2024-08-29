# PrintAppV2

## Project Description
<p>
  This VB.NET application is designed to print formatted text from an input file. It provides advanced text formatting capabilities, allowing users to customize the appearance of printed documents through simple formatting tokens. The application supports nested formatting, enabling users to apply different styles, fonts, and alignments within the same document.

</p>

## Features

*  **Advanced Text Formatting:** Customize text appearance with options for font name, style, size, margins, and alignment.
*  **Nested Formatting:** Supports the application of multiple formats within a document, with the ability to revert to previous formats.
*  **Error Handling:** Logs any processing errors in an error.txt file for troubleshooting.
*  **Templating Integration:** Can be used in conjunction with templating engines like Jinja, allowing dynamic data to be included in printed documents.

## How Users Can Use It
1.  **Input File:** Users specify a text file with the content and formatting tokens as a command-line argument.
2.  **Formatting Tokens:** The application recognizes the following tokens:
    *  `#@-` to begin a formatting block.
    *  `-#@` to end a formatting block.
3.  Customizable Formatting: Within the formatting blocks, users can specify formatting parameters such as `fontName`, `style`, `fontSize`, `marginTop`, `marginLeft`, and `content`.
4.  **Running the Application:** Users can run the application from the command line:

    > PrintAppV2.exe input.txt
    
    This will read the input.txt file, apply the specified formats, and print the content.
