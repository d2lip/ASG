module GDL
{
     language GDL
    {
        // --- Start of main parsing language ---
        syntax Main 
            = g:GestureDefinitions
            =>g;
    
        syntax GestureDefinitions
            = g:GestureDefinition
                =>[g]
            | list:GestureDefinitions "," g:GestureDefinition
                => [valuesof(list), g];
            
        syntax GestureDefinition
            = n:GestureName v:ValidationBlocks r:Returns 
            => Gesture{Name=>n, ValidateTokens=>v, Returns=>r};
            
        // -- Gesture Name --
        syntax GestureName
            = "name:" x:ValidName
            => x;
            
        // -- Validation Block --
        syntax ValidationBlocks
            = v:ValidationBlock* =>v;
            
        syntax ValidationBlock
            = "validate" "as" n:ValidName p:PrimitiveCondition* =>Validate{PrimitiveConditions=>p, Name=>n}
            | "validate" p:PrimitiveCondition* =>Validate{PrimitiveConditions=>p, Name=>""};
            
        // -- Return --
        syntax Returns
            = "return" k:Return* 
            =>k;
        
        syntax Return
            = k:SingleReturnType=>Return{ Name=>k };
         
         token SingleReturnType
            = r:ReturnTypes | r:ReturnTypes " "* "," => r;
         
         token ReturnTypes
            = r: "Distance changed" => r
            | r: "Slope changed" => r
            | r: "Acceleration" => r
            | r: "Distance" => r
            | r: "Position" => r
            | r: "Position changed" => r
            | r: "Pressure" => r
            | r: "Direction" => r
            | r: "Enclosed path" => r
            | r: "TouchID" => r
            | r: "Touch points" => r
            | r: "Info" ":" v:ValidName=>r+":"+v;
        
        /* Generic Rules */
        syntax PrimitiveConditions
            = x:PrimitiveCondition* 
            =>x;
                              
        syntax PrimitiveCondition
            = r:TouchState => r
            | r:TouchAreaRule => r
            | r:TouchTime => r
            | r:TouchLimitRule => r
            | r:TouchPathLength => r
            | r:TouchStepRule => r
            | r:ClosedLoop => r
            | r:EnclosedArea => r
            | r:OnSameObject => r
            | r:DistanceBetweenPoints => r
            | r:TouchPathBoundingBox =>r
            | r:TouchDirection =>r
            | r:TouchShape =>r;
        
        /* Touch shape */
        syntax TouchShape
            = "Touch shape" ":" s:Shape
            =>TouchShape{Values=>s};
        token Shape
            = x: "Line" => x
            | x: "Box" => x
            | x: "Circle" => x;
            
        /* Touch direction */
         syntax TouchDirection
            = "Touch direction" ":" d:DirectionOptions
            =>TouchDirection{Values=>d};
            
         token DirectionOptions
            = x: "Left" =>x
            | x: "Up" =>x
            | x: "Down" => x
            | x: "Right" => x;
            
        /* TouchPathBoundingBox */
         syntax TouchPathBoundingBox
            = "Touch path bounding box" ":" minH:ValidNum "x" minW:ValidNum ".." maxH:ValidNum "x" maxW:ValidNum
            =>TouchPathBoundingBox{MinHeight=>minH, MinWidth=>minW, MaxHeight=>maxH, MaxWidth=>maxW};
               
        /* Distance between points */    
        syntax DistanceBetweenPoints
            = r:DistanceBetweenPoints_Range =>r
            | r:DistanceBetweenPoints_UnChanged =>r
            | r:DistanceBetweenPoints_Behaviour =>r;
         
         syntax DistanceBetweenPoints_Range
            = "Distance between points" ":" min:ValidNum ".." max:ValidNum
            => DistanceBetweenPoints{Behaviour=>"range", Min=>min, Max=>max};
         
         syntax DistanceBetweenPoints_UnChanged
            = "Distance between points" ":" "unchanged" p:ValidNum "%"
            => DistanceBetweenPoints{Behaviour=>"unchanged", Min=>p};
            
         syntax DistanceBetweenPoints_Behaviour
            = "Distance between points" ":" x:DistanceBetweenPointsOptions
            => DistanceBetweenPoints{Behaviour=>x};
         
         token DistanceBetweenPointsOptions
            = x: "increasing" => x 
            | x: "decreasing" => x;   
            
               
        /* Touch(s) on Same object */    
         syntax OnSameObject
            = "On same object"
            =>OnSameObject{Condition=>"true"};
               
        /* Rule: Enclosed area*/   
         syntax EnclosedArea
            = "Enclosed area" ":" x:ValidNum ".." y:ValidNum
            => EnclosedArea{Min=>x, Max=>y};
            
        /* Rule: Closed loop */
        syntax ClosedLoop
            = "Closed loop"
            => ClosedLoop{State=>"true"};
            
        /* Rule: Touch Limit */
        syntax TouchLimitRule
            = r:FixedValue => r
            | r:Range => r;
        
        syntax Range
            = "Touch limit" ":" x:ValidNum ".." y:ValidNum
            =>TouchLimit{Type=>"Range", Min=>x, Max=>y};
            
        syntax FixedValue
            = "Touch limit" ":" x:ValidNum
            =>TouchLimit{Type=>"Fixed", Min=>x};
        
        /* Rule: Touch Step */ 
        syntax TouchStepRule
            = "Touch step" ":" x:ValidNum "touches" "within"
              y:ValidNum z:TouchStepUnitsForTime
            => TouchStep{TouchCount=>x, TimeLimit=>y, Unit=>z};
        
        token TouchStepUnitsForTime
            = "sec" | "msec";
            
            
        /* Touch Area */
        syntax TouchAreaRule
            = t:CircularArea => t
            | t:RectangleArea => t 
            | t:EllipseArea => t;
            
        syntax EllipseArea
            = "Touch area" ":" "Ellipse" x:ValidNum "x" y:ValidNum => TouchArea{Type=>"Ellipse", Value=>x+"x"+y};
        
        syntax RectangleArea
            = "Touch area" ":" "Rect" x:ValidNum "x" y:ValidNum => TouchArea{Type=>"Rect", Value=>x+"x"+y}
            | "Touch area" ":" "Rect" x:ValidNum "x" y:ValidNum "including" "last" h:ValidNum "touch" "within" time:ValidNum "sec"
            => TouchArea{Type=>"Rect", Value=>x+"x"+y, HistoryLevel=>h, HistoryTimeLine=>time};
        
        syntax CircularArea
            = "TouchArea" ":" "Circle" x:ValidNum
            => TouchArea{Type=>"Circle", Value=>x};
            
       /* Touch Time */
       syntax TouchTime
            = "Touch time" ":" val:ValidNum unit:TouchTimeUnit
            =>TouchTime{Value=>val, Unit=>unit };
            
        token TouchTimeUnit
            = "sec" | "secs" | "msec" | "msecs";
            
            
        /* Touch State */
        syntax TouchState
            = "Touch state" ":" opt:TouchStateOptions
            =>TouchState{States=>opt};
            
        syntax TouchStateOptions
            = opt:TouchStateOption+
            => opt;
            
        token TouchStateOption
            = "TouchUp" | "TouchDown" | "TouchMove";
          
        /* Touch Path Length */
        syntax TouchPathLength
            = "Touch path length:" min:ValidNum ".." max:ValidNum
            => TouchPathLength{Min=>min, Max=>max};
        
        /* Common tokens */
        token Digit
            = d:"0".."9" =>d;    
        token Boolean
            = "true"|"false";
        token ValidName
            = ("0".."9"|"a".."z"|"A".."Z"|"-"|"_")+;
        token ValidNum
            = ("0".."9")+;
            
        token NewLineCharacter = "\u000A" // New Line
                                | "\u000D" // Carriage Return
                                | "\u0085" // Next Line
                                | "\u2028" // Line Separator
                                | "\u2029"; // Paragraph Separator
      
        
        token CommentDelimited = "/*" CommentDelimitedContent* "*/";
        token CommentDelimitedContent = !("*") | "*"  !("/");
        token CommentLine = "//" CommentLineContent*;
        token CommentLineContent = !NewLineCharacter;
 
        /* interleave tokens */
        interleave AND = "and";
        interleave Space = " ";
        interleave CarriageReturn = '\u000D';
        interleave Tab = '\t';
        interleave ParagraphSeparator  = '\u2029';
        interleave NewLine = "\u000A";
        interleave Comment = CommentDelimited
                            | CommentLine;
 
    
    
        // Final tokens
        @{Classification["Keyword"]} final token Identifier = "as";
        @{Classification["Keyword"]} final token OrToken = "or";
        @{Classification["Keyword"]} final token AndToken = "and";
        @{Classification["Keyword"]} final token NameToken = "name:";
        @{Classification["Keyword"]} final token ValidationStateToken = "validate";
        @{Classification["Keyword"]} final token EndToken = "end";
        @{Classification["Keyword"]} final token ReturnToken = "return";
        
    }
}