Grayden Hormes
A00925756
COMP 3931/3770 Course Project 2015

Notes:
The .DLL/.lib files need to be in the same directory as the
WaveEditor.exe.

foward DFT is threaded using Tasks, which are better supported
than the rudimentary implemenation of threads that C# has. Both
Tasks and Threads are found in System.Threading, and are essentially
the same thing. A task will create a new thread when it is started

Things added since being marked:
1) mutiple instances of the program can interact by copy/cut/pasting
to another instance.
2) the user can now select multiple different quantization levels