# Compile-time Check Typescript

Using C# to control Angular validation and controls

[Medium Article](https://medium.com/@nullref/compile-time-checked-typescript-db5b60125d00)

This demo project uses C# validation attributes and custon Swagger generation to define compile-tim checked Typescript. The Angular controls and HTML are defined from object metadata constants emitted from the Swagger document including nullable, default, allow sort, data type, readonly, maxlength, minlength, required, identifier, display name, and description (tooltip).

This allows you to have Tyepscript code that will break and not compile if your C# objects are modified. Also, all validation is passed down from the C# definitions into Typescript. The form validators including required and max lengths are auto-generated from calls in TS files. HTML prompts, tooltips, and bindings are defined with generated constants that expose a problem immediately if the C# backing objects change.

<br/><br/>
<p align="center">
 <b>Standard Angular Forms Binding</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/e585ef69-6168-4058-b142-f67270da5d57)


<br/><br/>
<p align="center">
 <b>Angular Binding with generated metadata</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/3d3a7ba3-fceb-4b21-bd69-26218bc26c88)


<br/><br/>
<p align="center">
 <b>Standard Angular HTML</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/a6664336-2d1c-4f8d-97dd-9b9c42fc6a9d)


<br/><br/>
<p align="center">
 <b>Angular HTML with generated metadata</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/79d8f108-cfbe-4c68-972f-89d21929bc5c)


