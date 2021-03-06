;; -*- mode: lisp; encoding: utf-8; -*-

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; ukol4.lisp - základní definice v prototypovém jazyce
;;;;

;; (Vše dávat postupně do Listeneru)

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; Úkol 9.1
;;;;

;; Logika
[true add "OR" :value true]
[false add "OR" :value {(arg1) arg1}]
[true add "NOT" :value false]
[false add "NOT" :value true]

#|
[[true or true] name]
[[true or false] name]
[[false or true] name]
[[false or false] name]
[[true not] name]
[[false not] name]
|#



;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; Úkol 9.2
;;;;

;;Is nihil
[nihil add "IS-NIHIL" :value true]
[object add "IS-NIHIL" :value false]

#|
[[nihil is-nihil] name]
[[true is-nihil] name]
[[object is-nihil] name]
[[lobby is-nihil] name]
|#


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; Úkol 9.3
;;;;

;Seznamy
[lobby add "PAIR" :value [object clone]]
[pair set-name "PAIR"]
[pair add "CAR"]
[pair add "CDR"]
[pair add "CONS" :value {(arg1 arg2) [self set-car arg1] [self set-cdr arg2]}]
[lobby add "EMPTY-LIST" :value nihil]
[empty-list add "LENGTH" :value 0]


#|
[lobby add "PAR1" :value [pair clone]]
[par1 set-car 1]
[par1 set-cdr 2]
[par1 cons 1 :arg2 [[pair clone] cons 2 :arg2 [[pair clone] cons 7 :arg2 empty-list]]]
|#

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; Úkol 9.4
;;;;

[pair add "LENGTH" :value {() [[[self cdr] is empty-list]
                               if-true {() [1 esoteric]}
                               :else {() [[1 esoteric] + [[self cdr] length]]}]}]

#|
[lobby add "PAR1" :value [pair clone]]
[par1 cons 1 :arg2 [[pair clone] cons 2 :arg2 [[pair clone] cons 7 :arg2 empty-list]]]
[[par1 length] name]
|#

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; Úkol 9.5
;;;;

;;Operace na seznamech
[pair add "APPEND" :value {(arg1) [[[self cdr] is empty-list]
                               if-true {() [self set-cdr arg1]}
                               :else {() [[self cdr] append arg1]}]}] 

[pair add "FIND" :value {(arg1) [[[self car] is arg1]
                                      if-true {() [self car]}
                                      :else {() [[[self cdr] is empty-list]
                                                 if-true {() nihil}
                                                 :else {() [[self cdr] find arg1]}]}]}]


#|
[lobby add "PAR1" :value [pair clone]]
[lobby add "PAR2" :value [pair clone]]
[par1 cons 1 :arg2 [[pair clone] cons 2 :arg2 [[pair clone] cons 7 :arg2 empty-list]]] ;length = 3
[par2 cons 3 :arg2 [[pair clone] cons 4 :arg2 [[pair clone] cons 8 :arg2 empty-list]]]
[par1 append par2]
[[[[[par1 cdr] cdr] cdr] cdr] car] ;; 4
[par1 find 2]
[[par1 find 5] name] ;; vrati "nihil", kdyz 5 neni v seznamu
|#


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;
;;; Úkol 9.8  
;;; 

[zero add "*" :value {(arg1) self}]
[one add "*" :value {(arg1) arg1}]
[[one succ] add "*" :value {(arg1) [self + [[arg1 super] * self]]}]

#|
[[[0 esoteric] * [4 esoteric]] name]
[[[2 esoteric] * [1 esoteric]] name]
[[[2 esoteric] * [5 esoteric]] name]
|#



[zero add "-" :value {(arg1) [[[arg1 name] is 0]
                              if-true {() self}
                              :else {}]}]


[one add "-" :value {(arg1) [[arg1 is self]
                              if-true {() zero}
                              :else {() [[[arg1 name] is 0]
                                         if-true {() self}
                                         :else {}]}]}]

[[one succ] add "-" :value {(arg1) [[arg1 is self]
                              if-true {() zero}
                              :else {() [[[arg1 name] is 0]
                                         if-true {() self}
                                         :else {() [[self super] - [arg1 super]]}]}]}]

#|
[[[10 esoteric] - [4 esoteric]] name]
[[[2 esoteric] - [1 esoteric]] name]
[[[2 esoteric] - [5 esoteric]] name] ;vrati 0: protoze zaporna cisla nejsou definovana
|#

[zero add "^" :value {(arg1) self}]
[one add "^" :value {(arg1) [[[arg1 name] is 0]
                              if-true {() one}
                               :else {() [self * [self ^ [arg1 super]]]}]}]

#|
[[[0 esoteric] ^ [4 esoteric]] name]
[[[2 esoteric] ^ [1 esoteric]] name]
[[[4 esoteric] ^ [5 esoteric]] name]
[[[4 esoteric] ^ [0 esoteric]] name]
|#



[zero add "!" :value [1 esoteric]]
[one add "!" :value {(self) [self * [[self super] !]]}]


#|
[[[0 esoteric] !] name]
[[[2 esoteric] !] name]
[[[4 esoteric] !] name]
|#
