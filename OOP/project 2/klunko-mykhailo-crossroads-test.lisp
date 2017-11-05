;; -*- mode: lisp; encoding: utf-8; -*-

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; crossroads.lisp - obsahuje třidy crossroads a semaphore pro vytvoření  
;;;; obrázku křižovatky s několika semafory pro auta a chodce.
;;;;
;;;;

#|

DOKUMENTACE
-----------

Třída crossroads je potomkem třídy picture. Její instance představují jednoduchý obrázek křižovatky s několika semafory pro auta a chodce, kde je možný měnit fáze křižovatky(semaforů křižovatky pomoci metod). 

NOVÉ VLASTNOSTI

crossroads-phase:   Jednotlivé fáze křižovatky.
phase-count:        Počet fází.
program:            Program křižovatky

UPRAVENÉ ZDĚDĚNÉ ZPRÁVY

žádné

NOVÉ ZPRÁVY

set-crossroads-phase      Nastaví číslo fáze (parametr - číslo)
next-phase                Přepíná fáze (bez parametru)

Projděte si testovací kód na konci třidy.
|#

(defvar *program*
  '((t nil) (t t) (t t) (t nil)))



(defun crossroad-desc ()
  (let ((top-l (make-instance 'polygon))
        (top-r (make-instance 'polygon))
        (bot-l (make-instance 'polygon))
        (bot-r (make-instance 'polygon))
        (crosswalk1 (make-instance 'crosswalk))
        (crosswalk2 (make-instance 'crosswalk))
        (crosswalk3 (make-instance 'crosswalk))
        (sem-v (make-instance 'semaphore))
        (sem-p (make-instance 'semaphore)))       
    (set-items top-l (make-crossroad '((250 20) (250 250) (20 250))))
    (set-items top-r (make-crossroad '((350 20) (350 250) (580 250))))
    (set-items bot-l (make-crossroad '((20 350) (250 350) (250 580))))
    (set-items bot-r (make-crossroad '((580 350) (350 350) (350 580))))
    (set-semaphore-type sem-v :vehicle)
    (set-semaphore-type sem-p :pedastrian)    
    ;vehicle
    (move sem-v 350 350)
    ;pedastrian
    (move sem-p 90 160)
    (move crosswalk2 0 30)
    (move crosswalk3 0 60)
    (set-closedp top-l nil)
    (set-closedp top-r nil)
    (set-closedp bot-l nil)
    (set-closedp bot-r nil)
    (set-color top-l :black)
    (set-color top-r :black)
    (set-color bot-l :black)
    (set-color bot-r :black)
    (list sem-v sem-p top-l top-r bot-l bot-r crosswalk1 crosswalk2 crosswalk3)))

;;;
;;;Crossroads
;;;

(defclass crossroads (picture) 
  ((crossroads-phase :initform 0)
   (phase-count :initform 4)
   (program)))

;returns list of semaphore next-phase values
(defmethod actual-phase ((c crossroads))
  (nth (crossroads-phase c) (program c)))

(defmethod phase-len ((c crossroads))
  (length (actual-phase c)))

(defmethod crossroads-phase ((c crossroads))
  (slot-value c 'crossroads-phase))

(defmethod set-crossroads-phase ((c crossroads) value)
  (dotimes (x (phase-len c))
    (if(nth x (actual-phase c)) 
        (next-phase (nth x (items c)))))
  (setf (slot-value c 'crossroads-phase) value))

(defmethod phase-count ((c crossroads))
  (slot-value c 'phase-count))

(defmethod set-phase-count ((c crossroads) val)
  (setf (slot-value c 'phase-count) val))

(defmethod program ((c crossroads))
  (slot-value c 'program))

(defmethod set-program ((c crossroads) prog-list)
  (setf (slot-value c 'program) prog-list))

(defmethod next-phase ((c crossroads))
  (if (> (+ (crossroads-phase c) 1) (- (phase-count c) 1))
      (set-crossroads-phase c 0)
    (set-crossroads-phase c (+ (crossroads-phase c) 1))))

(defun make-crossroad (coord-list)
  (mapcar (lambda (coords)
            (apply #'move (make-instance 'point) coords))
          coord-list))

(defmethod initialize-instance ((cr crossroads) &rest initargs)
  (call-next-method)
  (set-program cr *program*)
  (set-items cr (crossroad-desc))
  cr)

#|
;; Testy. Všechny je vhodné si vyzkoušet. Nejlépe tak, že postupně
;; umístíte kurzor na každý výraz a vyhodnotíte (ve Windows F8)

(setf cr (make-instance 'crossroads))
(setf w (make-instance 'window))
(set-shape w cr)
(next-phase cr)
|#

#|

DOKUMENTACE
-----------

Třída semaphore je potomkem třídy picture. Její instance představují dopravní semafor. 

NOVÉ VLASTNOSTI

semaphore-type:     Druh semaforu(pro chodce nebo auta)
crossroads-phase:   Jednotlivé fáze semafora.
phase-count:        Počet fází.


UPRAVENÉ ZDĚDĚNÉ ZPRÁVY

žádné

NOVÉ ZPRÁVY

set-semaphore-type       Nastaví druh semaforu(parametr :pedastrian nebo :vehicle)
set-semaphore-phase      Nastaví číslo fáze (parametr - číslo)
next-phase               Přepíná fáze (bez parametru)

Projděte si testovací kód na konci třidy.
|#

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;
;;;Semaphore
;;;
(defvar *semaphores*
  '(((:red :orange :green) ((t nil nil) (t t nil) (nil nil t) (nil t nil)))
    ((:red :green) ((t nil) (nil t)))
    ;;custom-type of 4 lights
    ((:red :blue :orange :green)
              ((t nil nil nil) (t t nil nil) (nil t t nil) (nil nil nil t)
              (nil t nil t) (nil t t t) (t nil t t)))))

(defvar *box-width* 36)

(defun type-find (count)
  (let ((result (find-if (lambda (sem)
                           (= (length (first sem)) count))
                         *semaphores*)))
    (if (not result)
        (error "Semaphore doesn`t exist")
      result)))

(defun light-count (desc)
  (length (first desc)))

(defun return-phase-cycle (descr)
  (second descr))

(defun return-phase (light-count phase)
  (let ((result (return-phase-cycle (type-find light-count))))
    (nth phase result)))

(defun sem-phase-count (descr)
  (length (return-phase-cycle descr)))

(defun make-sem-light (position color)
  (let ((result (move (set-on-color (set-radius (make-instance 'light) 
                            18)
                              color) 
                      (* 1/2 *box-width*)
                      (+ (* 1/2 *box-width*)
                         (* position *box-width*)))))
    result))


  
(defun make-light-list (desc)
  (let ((colors (first desc))
        result)
    (dotimes (i (light-count desc))
      (setf result (cons (make-sem-light i (nth i colors)) result)))
    (reverse result)))
      
(defun light-items (desc)
  (set-items (make-instance 'picture) (make-light-list desc)))

(defclass semaphore (picture) 
  ((semaphore-type :initform :custom)
   (semaphore-phase :initform 1)))

(defun set-light-phase (lights phase)
  (dolist (pair (mapcar #'cons lights phase))
    (set-onp (car pair) (cdr pair))))

(defmethod sem-light-count ((s semaphore))
  (length (items (first(items s)))))

(defmethod semaphore-phase ((s semaphore))
  (slot-value s 'semaphore-phase))

(defmethod set-semaphore-phase ((s semaphore) value)
  (set-light-phase (items (first(items s)))
                   (return-phase (sem-light-count s) value))
  (setf (slot-value s 'semaphore-phase) value)
  s)

(defmethod phase-count ((s semaphore))
  (slot-value s 'phase-count))

(defmethod semaphore-type ((s semaphore))
  (sem-phase-count (type-find (sem-light-count s))))

(defmethod set-semaphore-type ((s semaphore) value)
  (setf (slot-value s 'semaphore-type) value)
  (cond 
   ((eq value :vehicle) (set-light-count s 3))
   ((eq value :pedastrian) (set-light-count s 2))
   (t (error "Unknown type"))))

(defmethod set-light-count ((s semaphore) count)
  (set-items s
             (list (light-items (type-find count))
             (sem-make-base count)))
  (set-semaphore-phase s 0)
  s)

(defmethod next-phase ((s semaphore))
  (if (> (+ (semaphore-phase s) 1) (- (phase-count s) 1))
      (set-semaphore-phase s 0)
    (set-semaphore-phase s (+ (semaphore-phase s) 1)))) 


(defun semaphore-base (count)
  (list (make-instance 'point)
        (move (make-instance 'point) *box-width* 0)
        (move (make-instance 'point) 
              *box-width*
              (* count *box-width*))
        (move (make-instance 'point) 0 (* count *box-width*))))

(defun sem-make-base (count)
  (set-items (make-instance 'polygon)
   (semaphore-base count)))
         
(defmethod initialize-instance ((s semaphore) &rest args)
  (call-next-method)
  (set-light-count s 2))

#|
;; Testy. Všechny je vhodné si vyzkoušet. Nejlépe tak, že postupně
;; umístíte kurzor na každý výraz a vyhodnotíte (ve Windows F8)

(setf s (make-instance 'semaphore))
(setf w (make-instance 'window))
(set-shape w s)
(next-phase s)

(set-semaphore-type s :vehicle)
(next-phase s)

;;custom type of semaphore(should exisit in variable *semaphores*)
(set-light-count s 4)
(next-phase s)
|#


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;
;;;Crosswalk
;;;

;;;Pomocná třida pro malování přechodu pro chodce. Je potomkem třídy picture.

(defclass crosswalk (picture) ())

(defmethod initialize-instance ((cw crosswalk) &rest initargs)
  (call-next-method)
  (let ((crosswalk1 (make-instance 'polygon)))
    (set-items crosswalk1 (make-crossroad '((150 275) (150 255) (245 255) (245 275))))
    (set-color crosswalk1 :black)
    (set-items cw (list crosswalk1))))
    
(defun make-base (coord-list filledp closedp color)
  (set-closedp (set-filledp
                (set-color
                 (set-items (make-instance 'polygon)
                            (mapcar (lambda (pair)
                                      (apply #'make-point pair))
                                    coord-list))
                 color)
                filledp)
               closedp))

(defun make-point (x y)
  (move (make-instance 'point) x y))