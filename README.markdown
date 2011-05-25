# Html5EditorFor

Html5EditorFor is an extension of default MVC 3 EditorFor, which allows to:

- Use custom attributes in View: `Html.Html5EditorFor(model => model, null, attributes: new { style = "font-weight: bold" })`
- Generate [HTML 5 Input type] (http://www.w3schools.com/html5/html5_form_input_types.asp) based on:
 - type of the property (int, float, double etc.)
 - DataType attribute form System.ComponentModel.DataAnnotations
 - using custom Html5*Attribute from library over ViewModel

Additionally, it extends default input classes to include 'numeric-int' for not decimal numbers and 'numeric-float' for decimal numbers. This allows to use jQuery plugin like [numeric](http://www.texotela.co.uk/code/jquery/numeric/) i.e.: `$('.numeric-int').numeric()`

Code on github contains:

- Source code for Html5EditorFor
- Sample web site
 
Library is avaliable on [NuGet](http://www.nuget.org/Package/Html5EditorFor/1.0)