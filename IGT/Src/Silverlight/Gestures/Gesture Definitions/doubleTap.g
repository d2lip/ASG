name: DoubleTap

validate as step1
    Touch state: TouchUp
validate as step2
    Touch state: TouchUp	
validate
    Touch limit: 1
	On same object:true
return
    Touch points