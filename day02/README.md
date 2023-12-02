Find max of each color on each line
```nvim
:%s/\v:(.*) (\d+) red/:max([\2])#\1\<CR>
:%s/\v\]\)#(.*) (\d+) red/,\2])#\1\<CR>
99@:
:%s/\v#(.*) (\d+) green/;max([\2])#\1\<CR>
:%s/\v\]\)#(.*) (\d+) green/,\2])#\1\<CR>
99@:
:%s/\v#(.*) (\d+) blue/;max([\2])#\1\<CR>
:%s/\v\]\)#(.*) (\d+) blue/,\2])#\1\<CR>
99@:
:%s/\v:(.*);(.*);(.*)#.*/\=printf(':%i,%i,%i',eval(submatch(1)),eval(submatch(2)),eval(submatch(3)))\<CR>
```
Part 1, output ids where colors "fit" the requirements
```nvim
:%s/\vGame (\d+):(\d+),(\d+),(\d+)\n/\=submatch(2)<=12&&submatch(3)<=13&&submatch(4)<=14?printf('+%i',submatch(1)):''\<CR>
```
Part 2, output the product of each lines colors
```
:%s/\v.*:(\d+),(\d+),(\d+)\n/\=printf('%+i',submatch(1)*submatch(2)*submatch(3))\<CR>
```
Sum the numbers
```
:s/\v(.*)/\=eval(submatch(1))\<CR>
```
