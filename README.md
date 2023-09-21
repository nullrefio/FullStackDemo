# Compile-time Check Typescript

Using C# to control Angular validation and controls

[Medium Article](https://medium.com/@nullref/compile-time-checked-typescript-db5b60125d00)

This demo project uses C# validation attributes and custon Swagger generation to define compile-tim checked Typescript. The Angular controls and HTML are defined from object metadata constants emitted from the Swagger document including nullable, default, allow sort, data type, readonly, maxlength, minlength, required, identifier, display name, and description (tooltip).

This allows you to have Tyepscript code that will break and not compile if your C# objects are modified. Also, all validation is passed down from the C# definitions into Typescript. The form validators including required and max lengths are auto-generated from calls in TS files. HTML prompts, tooltips, and bindings are defined with generated constants that expose a problem immediately if the C# backing objects change.

<br/><br/>
<p align="center">
 <b>Standard Angular Forms Binding</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/86a31788-258b-41a2-9145-f8b9f80b9797)

<br/><br/>
<p align="center">
 <b>Angular Binding with generated metadata</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/abcb2fb6-61bb-4833-a987-fc2b3549235c)


<br/><br/>
<p align="center">
 <b>Standard Angular HTML</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/a94abaa7-3cab-444b-bfe5-5b16d62fdb75)


<br/><br/>
<p align="center">
 <b>Angular HTML with generated metadata</b>
</p>

![image](https://github.com/nullrefio/FullStackDemo/assets/7587796/cf64cb59-3eb9-48ee-b51e-177564ccc898)

