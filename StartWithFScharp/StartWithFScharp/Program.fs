

open System.Drawing
open System
open OpenCvSharp

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    
    //let fileName = "C:\Users\karl\OneDrive\ALL\UNIF\2eme - Mémoire\Echantillon.PNG"
    let fileName = "C:\Users\kwilvers\OneDrive\ALL\UNIF\2eme - Mémoire\Ech.PNG"

    let image = Cv2.ImRead(fileName, ImreadModes.GrayScale)
    let output = InputOutputArray.Create(image.Clone())
    //let gray = Cv2.CvtColor(image, output, ColorConversionCodes.BGR2GRAY)
    let input = InputArray.Create(image)
    Cv2.GaussianBlur(input, output, Size(9,9), 2.0,2.0)
    //let circles = Cv2.HoughCircles(input, HoughMethods.Gradient, 1.0, 5.0, 200.0, 100.0, 0, 0)
    input = InputArray.Create(output.GetMat())
    let circles = Cv2.HoughCircles(input, HoughMethods.Gradient, 1.0, 5.0, 200.0, 100.0, 0, 0)
    //let lines = Cv2.HoughLines(output, 1.0, Math.PI/180.0, 50, 0.0, 0.0)

    if circles = null 
    then
        for c in circles do
            Cv2.Circle(output :?> InputOutputArray, Convert.ToInt32(c.Center.X), Convert.ToInt32(c.Center.Y), Convert.ToInt32(c.Radius), Scalar(0.0, 255.0, 0.0), 4, LineTypes.AntiAlias, 0)
    
    Cv2.ImShow("output", output.GetMat())
    Cv2.WaitKey(0) |> ignore

    (*let image = cv2.imread(args["image"])
    let output = image.copy()
    let gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)*)

    //let a = new Library1

    (*  
    // single line comments use a double slash
    (* multi line comments use (* . . . *) pair

    -end of multi line comment- *)

    // ======== "Variables" (but not really) ==========
    // The "let" keyword defines an (immutable) value
    let myInt = 5
    let myFloat = 3.14
    let myString = "hello"   //note that no types needed

    // ======== Lists ============
    let twoToFive = [2;3;4;5]        // Square brackets create a list with
                                     // semicolon delimiters.
    let oneToFive = 1 :: twoToFive   // :: creates list with new 1st element
    // The result is [1;2;3;4;5]
    let zeroToFive = [0;1] @ twoToFive   // @ concats two lists

    // IMPORTANT: commas are never used as delimiters, only semicolons!

    // ======== Functions ========
    // The "let" keyword also defines a named function. *)
    //let square x = x * x          // Note that no parens are used.
    //square 3                      // Now run the function. Again, no parens.
    (*
    let add x y = x + y           // don't use add (x,y)! It means something
                                  // completely different.
    add 2 3                       // Now run the function.

    // to define a multiline function, just use indents. No semicolons needed.
    let evens list =
       let isEven x = x%2 = 0     // Define "isEven" as a sub function
       List.filter isEven list    // List.filter is a library function
                                  // with two parameters: a boolean function
                                  // and a list to work on

    evens oneToFive               // Now run the function

    // You can use parens to clarify precedence. In this example,
    // do "map" first, with two args, then do "sum" on the result.
    // Without the parens, "List.map" would be passed as an arg to List.sum
    let sumOfSquaresTo100 =
       List.sum ( List.map square [1..100] )

    // You can pipe the output of one operation to the next using "|>"
    // Here is the same sumOfSquares function written using pipes
    let sumOfSquaresTo100piped =
       [1..100] |> List.map square |> List.sum  // "square" was defined earlier

    // you can define lambdas (anonymous functions) using the "fun" keyword
    let sumOfSquaresTo100withFun =
       [1..100] |> List.map (fun x->x*x) |> List.sum

    // In F# there is no "return" keyword. A function always
    // returns the value of the last expression used.

    // ======== Pattern Matching ========
    // Match..with.. is a supercharged case/switch statement.
    let simplePatternMatch =
       let x = "a"
       match x with
        | "a" -> printfn "x is a"
        | "b" -> printfn "x is b"
        | _ -> printfn "x is something else"   // underscore matches anything

    // Some(..) and None are roughly analogous to Nullable wrappers
    let validValue = Some(99)
    let invalidValue = None

    // In this example, match..with matches the "Some" and the "None",
    // and also unpacks the value in the "Some" at the same time.
    let optionPatternMatch input =
       match input with
        | Some i -> printfn "input is an int=%d" i
        | None -> printfn "input is missing"

    optionPatternMatch validValue
    optionPatternMatch invalidValue

    // ========= Complex Data Types =========

    //tuples are quick 'n easy anonymous types
    let twoTuple = 1,2
    let threeTuple = "a",2,true

    //record types have named fields
    type Person = {First:string; Last:string}
    let person1 = {First="john"; Last="Doe"}

    //union types have choices
    type Temp = 
        | DegreesC of float
        | DegreesF of float
    let temp = DegreesF 98.6

    //types can be combined recursively in complex ways
    type Employee = 
      | Worker of Person
      | Manager of Employee list
    let jdoe = {First="John";Last="Doe"}
    let worker = Worker jdoe

    let manager = Manager [worker;worker]

    // ========= Printing =========
    // The printf/printfn functions are similar to the
    // Console.Write/WriteLine functions in C#.
    printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true
    printfn "A string %s, and something generic %A" "hello" [1;2;3;4]

    // all complex types have pretty printing built in
    printfn "twoTuple=%A,\nPerson=%A,\nTemp=%A,\nEmployee=%A" 
             twoTuple person1 temp worker

    // There are also sprintf/sprintfn functions for formatting data
    // into a string, similar to String.Format.
    *)







    0 // return an integer exit code

