-- Bubblesort
size(256)
delay(2)
rainbow(false)
visual("Scatter Plot")
wait(1)
shuffle("Scramble")
wait(1)
delay(0.3)
sort("Bubble Sort")
verify()

-- Pause for 1 second
wait(1)

-- Quicksort
size(512)
delay(2.5)
rainbow(true)
visual("Circle")
wait(1)
shuffle("Scramble")
wait(1)
delay(10)
sort("Quick Sort")

-- Pause for 1 second again
wait(1)

-- Bogosort
size(8)
delay(100)
rainbow(false)
visual("Classic")
wait(1)
shuffle("Scramble")
wait(1)
delay(0.1)
sort("Bogo Sort")
delay(50)
verify()

print("Done!")