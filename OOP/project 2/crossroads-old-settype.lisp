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


UPRAVENÉ ZDĚDĚNÉ ZPRÁVY

žádné

NOVÉ ZPRÁVY

set-crossroads-phase      Nastaví číslo fáze (parametr - číslo)
next-phase                Přepíná fáze (bez parametru)

Projděte si testovací kód na konci třidy.
|#

(defvar *semaphores*
  '(((:red :green) ((t nil) (nil t)))
    ((:red :orange :green) ((t nil nil) (t t nil) (nil nil t) (nil t nil)))))

;;;
;;;Crossroads
;;;

(defclass crossroads (picture) 
  ((crossroads-phase :initform 1)
   (phase-count :initform 4)
   (items)
   (semaphores)
   (program)))

(defmethod crossroads-phase ((c crossroads))
  (slot-value c 'crossroads-phase))

(defmethod set-crossroads-phase ((c crossroads) value)
  (setf (slot-value c 'crossroads-phase) value)
  (cond ((eq value 1)
         (progn (set-semaphore-phase (fifth (items c)) 1) (set-semaphore-phase (sixth (items c)) 1)))
        ((eq value 2)
         (progn (set-semaphore-phase (fifth (items c)) 2) (set-semaphore-phase (sixth (items c)) 1)))
        ((eq value 3)
         (progn (set-semaphore-phase (fifth (items c)) 3) (set-semaphore-phase (sixth (items c)) 2)))
        ((eq value 4)
         (progn (set-semaphore-phase (fifth (items c)) 4) (set-semaphore-phase (sixth (items c)) 1)))))

(defmethod phase-count ((c crossroads))
  (slot-value c 'phase-count))

(defmethod next-phase ((c crossroads))
  (if (> (+ (crossroads-phase c) 1) (phase-count c))
      (set-crossroads-phase c 1)
    (set-crossroads-phase c (+ (crossroads-phase c) 1))))

(defun semaphore-v1-position ()
  (list (make-base '((360 360) (410 360) (410 470) (360 470)) nil t :black)))

(defun make-crossroad (coord-list)
  (mapcar (lambda (coords)
            (apply #'move (make-instance 'point) coords))
          coord-list))

(defmethod initialize-instance ((cr crossroads) &rest initargs)
  (call-next-method)
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
    (move sem-v 340 340)
    ;pedastrian
    (move sem-p 70 140)
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
    (set-items cr (list top-l top-r bot-l bot-r sem-v sem-p crosswalk1 crosswalk2 crosswalk3)))
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
(defvar *vehicle*
  '((:red :orange :green) ((t nil nil) (t t nil) (nil nil t) (nil t nil))))

(defvar *pedastrian*
  '((:red :green) ((t nil) (nil t))))

(defun light-count (desc)
  (length (first desc)))

(defun phase-count (desc)
  (length (second desc)))

(defun make-sem-light (color)
  (let ((result (set-on-color (set-radius (make-instance 'light) 
                            18)
                              color)))
    result))


  
(defun make-light-list (desc)
  (let ((colors (first desc))
        result)
    (dotimes (i (light-count desc))
      (setf result (cons (make-sem-light (nth i colors)) result)))
    (reverse result)))
      
    
    

(defclass semaphore (picture) 
  ((semaphore-type)
   (semaphore-phase :initform 1)
   (phase-count)))

#|

(defun red-v (semaphore)
  (set-on (first (items semaphore)))
  (set-off (second (items semaphore)))
  (set-off (third (items semaphore))))

(defun red-orn-v (semaphore)
  (set-on (first (items semaphore)))
  (set-on (second (items semaphore)))
  (set-off (third (items semaphore))))

(defun green-v (semaphore)
  (set-off (first (items semaphore)))
  (set-off (second (items semaphore)))
  (set-on (third (items semaphore))))

(defun orn-v (semaphore)
  (set-off (first (items semaphore)))
  (set-on (second (items semaphore)))
  (set-off (third (items semaphore))))

(defun p-red (semaphore)
  (set-on (first (items semaphore)))
  (set-off (second (items semaphore))))

(defun p-green (semaphore)
  (set-off (first (items semaphore)))
  (set-on (second (items semaphore))))
|#

(defmethod semaphore-phase ((s semaphore))
  (slot-value s 'semaphore-phase))

(defmethod set-semaphore-phase ((s semaphore) value)
  (setf (slot-value s 'semaphore-phase) value)
  (cond ((eq (semaphore-type s) :vehicle) 
         (cond ((eq value 1) (red-v s))
               ((eq value 2) (red-orn-v s))
               ((eq value 3) (green-v s))
               ((eq value 4) (orn-v s))
               (t (error "unknown phase"))))
        ((eq (semaphore-type s) :pedastrian)
         (cond ((eq value 1) (p-red s))
               ((eq value 2) (p-green s))
               (t (error "unknown phase"))))))

(defmethod phase-count ((s semaphore))
  (slot-value s 'phase-count))

(defmethod set-phase-count ((s semaphore) value)
  (setf (slot-value s 'phase-count) value))

(defmethod semaphore-type ((s semaphore))
  (slot-value s 'semaphore-type))

(defmethod set-semaphore-type ((s semaphore) value)
  (setf (slot-value s 'semaphore-type) value)
  (cond
     ((eq value :vehicle) (let ((v1 (make-instance 'light))
                                (v2 (make-instance 'light))
                                (v3 (make-instance 'light)))
                            (set-items s (list v1 v2 v3 (semaphore-base-vehicle)))
                            (set-phase-count s 4)
                            (set-radius v1 18)
                            (set-radius v2 18)
                            (set-radius v3 18)
                            (set-on-color (move v1 45 39) :red)
                            (set-off (set-on-color (move v2 45 75) :orange))
                            (set-off (set-on-color (move v3 45 111) :green))
                            (set-semaphore-phase s 1)))
     ((eq value :pedastrian) (let ((p1 (make-instance 'light))
                                   (p2 (make-instance 'light)))
                               (set-items s (list p1 p2 (semaphore-base-pedastr)))
                               (set-phase-count s 2)
                               (set-radius p1 18)
                               (set-radius p2 18)
                               (set-on-color (move p1 45 41) :red)
                               (set-off (set-on-color (move p2 45 79) :green))
                               (set-semaphore-phase s 1)))))

(defmethod next-phase ((s semaphore))
  (if (> (+ (semaphore-phase s) 1) (phase-count s))
      (set-semaphore-phase s 1)
    (set-semaphore-phase s (+ (semaphore-phase s) 1)))) 

(defun make-point (x y)
  (move (make-instance 'point) x y))

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

(defun semaphore-base-vehicle ()
  (make-base '((20 20) (70 20) (70 130) (20 130)) t t :black))

(defun semaphore-base-pedastr ()
  (make-base '((20 20) (70 20) (70 100) (20 100)) nil t :black))
         
(defmethod initialize-instance ((s semaphore) &rest args)
  (call-next-method)
  (let ((b (semaphore-base-vehicle)))
    (set-items s (list b)))
  s)

#|
;; Testy. Všechny je vhodné si vyzkoušet. Nejlépe tak, že postupně
;; umístíte kurzor na každý výraz a vyhodnotíte (ve Windows F8)

(setf s (make-instance 'semaphore))
(setf w (make-instance 'window))
(set-semaphore-type s :vehicle)
(set-shape w s)
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
    