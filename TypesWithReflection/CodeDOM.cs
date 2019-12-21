using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace TypesWithReflection
{
    public class CodeDOMExample
    {
        public void GenerateCodDOM()
        {
            CodeDOM cd = new CodeDOM();
            CodeCompileUnit ccu = cd.CodeCreateCompileUnit();
            cd.PublishDOM(ccu);
        }

    }
    public class CodeDOM
    {
        public CodeCompileUnit CodeCreateCompileUnit()
        {
            CodeCompileUnit ccu = new CodeCompileUnit();
            // Create a namespace to hold the types we are going to create
            CodeNamespace codeNamespace = new CodeNamespace("TypesWithRef");
            // Import the system namespace
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            //Create person class
            CodeTypeDeclaration personClass = new CodeTypeDeclaration("Person");
            personClass.IsClass = true;
            personClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
            // Add the Person class to namespace

            codeNamespace.Types.Add(personClass);
            // Create a field to hold the name of a person

            CodeMemberField nameField = new CodeMemberField("String", "_name");
            nameField.Attributes = MemberAttributes.Private;

            CodeMemberProperty nameProperty = new CodeMemberProperty()
            {
                Name = "Name",
                HasGet = true,
                HasSet = true,
                Attributes = MemberAttributes.Public
            };

            CodeMemberMethod displayNameMethod = new CodeMemberMethod()
            {
                Name = "DisplayName",
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = new CodeTypeReference("System.String"),

            };
            displayNameMethod.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "name"));

            displayNameMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("name")));
            // Add the name field to the Person class
            personClass.Members.Add(nameField);
            personClass.Members.Add(nameProperty);
            personClass.Members.Add(displayNameMethod);
            //Add the namespace to the document
            ccu.Namespaces.Add(codeNamespace);
            return ccu;

        }
        public void PublishDOM(CodeCompileUnit ccu)
        {
            // Now we need to send our document somewhere
            // Create a provider to parse the document
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            // Give the provider somewhere to send the parsed output
            StringWriter sw = new StringWriter();
            // Set some options for the parse - we can use the defaults
            CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions();
            // Generate the C# source from the CodeDOM
            codeDomProvider.GenerateCodeFromCompileUnit(ccu, sw, codeGeneratorOptions);
            // Close the output stream
            sw.Close();

            // Print the C# output
            Console.WriteLine(sw.ToString());
        }
    }
}
