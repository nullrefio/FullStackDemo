# Compile-time Check Typescript

Using C# to control Angular validation and controls

[Medium Article](https://medium.com/@nullref/compile-time-checked-typescript-db5b60125d00)

This demo project uses C# validation attributes and custon Swagger generation to define compile-tim checked Typescript. The Angular controls and HTML are defined from object metadata constants emitted from the Swagger document including nullable, default, allow sort, data type, readonly, maxlength, minlength, required, identifier, display name, and description (tooltip).

This allows you to have Tyepscript code that will break and not compile if your C# objects are modified. Also, all validation is passed down from the C# definitions into Typescript. The form validators including required and max lengths are auto-generated from calls in TS files. HTML prompts, tooltips, and bindings are defined with generated constants that expose a problem immediately if the C# backing objects change.

Standard Angular Forms Binding
![image](https://github.com/nullrefio/Demo.TypescriptCompiled/assets/7587796/9dbc1abd-5413-492a-899d-06fe3a3ab776)

Angular Binding with generated metadata
![image](https://github.com/nullrefio/Demo.TypescriptCompiled/assets/7587796/30fc87e0-d756-4141-84c8-96f4e7efaed7)

Standard Angular HTML
![image](https://github.com/nullrefio/Demo.TypescriptCompiled/assets/7587796/ea964c39-550c-4515-bf76-b0dbfed8a5ec)

Angular HTML with generated metadata
![image](https://github.com/nullrefio/Demo.TypescriptCompiled/assets/7587796/910a8d27-237c-4203-bf5a-2b9ee9d15c36)
