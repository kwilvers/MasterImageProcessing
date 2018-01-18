namespace MaSuperLib

type Library1() = 
    let square x = x * x  
    let add x y = x + y

    //Filtrer les nombre pair
    let evens list =
       let isEven x = x%2 = 0     
       List.filter isEven list

    member this.X = "F#"
