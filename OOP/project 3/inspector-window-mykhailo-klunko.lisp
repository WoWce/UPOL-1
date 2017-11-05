;; -*- mode: lisp; encoding: utf-8; -*-

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;;
;;;; Zdrojový soubor k 3. úkolu inspector-window
;;;;
;;;;

#| 
Před načtením souboru načtěte knihovnu micro-graphics
Pokud při načítání (kompilaci) dojde k chybě 
"Reader cannot find package MG",
znamená to, že knihovna micro-graphics není načtená.

Kromě standardních souborů vyžaduje načíst soubory 07_text-shape.lisp, 04_bounds.lisp

Projděte si testovací kód na konci souboru
|#

#|

DOKUMENTACE
-----------

TŘÍDA INSPECTED-WINDOW (WINDOW)
-------------------------------

Instance reprezentují prohlížená okna. Okno se otevře automaticky při vytváření nové instance.
Oknu lze nastavit vykreslované grafické objekty, o kterych chcete zobrazit informace ce třidě
inspector-window.


PŘEPSANÉ METODY

window-mouse-down

Posílá událost o kliknutí svému delegatovi.

change

Ve třídě inspected-window pošle delegatovi zprávu o změně objektu, aby se vyvolala aktualizace
zobrazene informaci ve třidě inspector-window. Volá zděděnou metodu.


TŘÍDA INSPECTOR-WINDOW (WINDOW)
-------------------------------
Instance reprezentují okna, zobrazující informace o zvoleném objektu jiného (prohlíženého) okna.
Okno se otevře automaticky při vytváření nové instance. 
Oknu lze nastavit okno, jež má být prohlíženo.

PRACE S TŘÍDAMI GRAFICKÝCH OBJEKTŮ, NEDEFINOVANYMÍ V STANDARTNIM SOUBORU
 
Pro třídy grafických objektů, které nejsou definované v souboru 07.lisp, máte definovat metodu,
která vypádat následovně:

;(defmethod properties ((class your-class))
;                 '(prop1, prop2...))

kde:

your-class    název vášé definované třidy
prop1, prop2  jména nastavítelných vlastností té třidy

Jestli třida se skláda s několika podobrazků, musíte pomocí následujíci metody nastavít hodnotu 
vlastnosti solidp na hodnotu t. Jinak inspector-window bude zobrazovát informací o kliknutém
podobrazku.

;(defmethod solidp ((class your-class))
;  t)


NOVÉ VLASTNOSTI

inspected-window   Odkaz na okno, jež má být prohlíženo. Po nastavení
                   prohlíženého okna prohlížeč zobrazí informace o tomto
                   okně. Po kliknutí do prohlíženého okna zobrazí prohlížeč 
                   informace o pevném objektu, na který uživatel klikl. 
                   Po kliknutí mimo všechny objekty se zobrazí opět 
                   informace o okně.

NOVÉ ZPRÁVY

set-inspected-window  

Vlastní nastavení vlastnosti inspected-window. Po nastavení vlastnosti prohlížeč
zobrazí informace o okně umíštěném v te vlastnosti a se objektu inspected-window
nastaví delegate na inspector-window.



PŘIJÍMANÉ UDÁLOSTI

set-background inspector-window color

Nastavení vlastnosti background.



PŘEPSANÉ METODY

install-callbacks

Posílá se oknu jako součást inicializace. Slouží k instalaci zpětných
volání knihovny micro-graphics. Ve třídě inspector-window instaluje zpětná volání
:double-click pomocí zpráv install-double-click-callback.


TŘÍDA PROP-TEXT-SHAPE (TEXT-SHAPE)
----------------------------------

Grafický objekt reprezentujicí text. 


|#
;;;
;;; Třída inspected-window
;;;

(defclass inspected-window (window)
  ())

(defmethod window-mouse-down ((w inspected-window) button position)
  (let ((shape (find-clicked-shape w position)))
    (if shape
        (progn 
          (mouse-down-inside-shape w shape button position)
          (send-event w 'show-obj position))
      (progn
        (mouse-down-no-shape w button position)
        (send-event w 'window-click-info)))))

(defmethod change ((i inspected-window) message changed-obj args)
  (call-next-method)
  (send-event i 'update-info changed-obj))

;;;
;;; Třída inspector-window
;;;

(defclass inspector-window (window)
  (inspected-window))

(defmethod set-inspected-window ((irw inspector-window) window)
  (setf (slot-value irw 'inspected-window) window)
  (set-delegate (inspected-window irw) irw)
  (set-shape irw (shape-info (slot-value irw 'inspected-window))))

(defmethod inspected-window ((irw inspector-window))
  (slot-value irw 'inspected-window))

(defun shape-info (shape)
  (let ((b (make-instance 'picture))
        (c nil)
        (info)
        (position 25)
        (cl))
    (move (set-text (setf cl (make-instance 'prop-text-shape)) (format nil "CLASS ~s" (type-of shape))) 0 10)
    (dolist (x (properties shape))
      (move (setf c (make-instance 'prop-text-shape)) 0 position)
      (setf position (+ position 15))
      (set-text c (format nil "~s = ~s" x (funcall x shape)))
      (set-object c shape)
      (set-property c x)
      (setf info (cons c info)))
    (setf info (cons cl info))
    (set-items b info)
    b))


(defmethod show-obj ((irw inspector-window) sender position)
  (set-shape irw (shape-info (find-clicked-shape sender position))))

(defmethod window-click-info ((irw inspector-window) sender)
  (set-shape irw (shape-info sender)))

(defmethod window-info ((irw inspector-window))
  (set-shape irw (shape-info irw)))

(defmethod update-info ((irw inspector-window) sender changed-obj)
  (set-shape irw (shape-info changed-obj)))

;;; Klikani

(defun dialog-window ()
  (first (multiple-value-list
   (capi:prompt-for-value "Zadejte novou hodnotu"))))

(defmethod window-double-click ((irw window) button position)
  (window-mouse-down irw button position)
  (let ((obj (find-clicked-shape irw position)))
    (if obj
        (if (property (find-clicked-shape irw position))
            (funcall (setter-name (property (find-clicked-shape irw position))) 
                     (object (find-clicked-shape irw position)) (dialog-window))))))

(defmethod install-double-click-callback ((irw inspector-window))
  (mg:set-callback 
   (mg-window irw) 
   :double-click (lambda (mgw button x y)
		 (declare (ignore mgw))
		 (window-double-click 
                  irw
                  button 
                  (move (make-instance 'point) x y)))))

(defmethod install-callbacks ((irw inspector-window))
  (call-next-method)
  (install-double-click-callback irw)
  irw)


(defun setter-name (property-name)
(find-symbol (format nil "SET-~a" property-name)))


#|
;;Testovaci kod
(setf irw (make-instance 'inspector-window))
(setf idw (make-instance 'inspected-window))
(setf cr (make-instance 'circle))
(move (setf a (make-instance 'point)) 20 20)
(move (setf b (make-instance 'point)) 50 20)
(move (setf c (make-instance 'point)) 50 40)
(move (setf d (make-instance 'point)) 20 40)
(move (setf e (make-instance 'point)) 15 15)
(setf pl (make-instance 'polygon))
(set-thickness pl 5)
(set-items pl (list a b c d))
(move (set-radius (set-thickness (set-color cr :darkslategrey) 20) 55)
      148
      100)
(set-items (setf pic (make-instance 'picture)) (list cr pl e)) 
(set-shape idw pic)
(set-inspected-window irw idw)

;;Testovaní hlášení změn ve třídě inspector-window
(set-color cr :lightblue)

;;Třida bulls-eye 
;;Pro testovani vyžaduje načíst soubor 07_bulls-eye.lisp
(setf be (make-instance 'bulls-eye))
(defmethod solidp ((be bulls-eye))
  t)
;;Metoda pro fungovaní inspector-window s třidou bulls-eye 
(defmethod properties ((b bulls-eye))
  '(radius item-count squarep))

(set-shape idw be)

|#

;;;
;;; Třída prop-text-shape
;;;

(defclass prop-text-shape (text-shape) 
  ((object :initform nil)
  (property :initform nil)))

(defmethod set-object ((shape prop-text-shape) obj)
  (setf (slot-value shape 'object) obj))

(defmethod object ((shape prop-text-shape))
  (slot-value shape 'object))

(defmethod property ((shape prop-text-shape))
  (slot-value shape 'property))

(defmethod set-property ((shape prop-text-shape) value)
  (setf (slot-value shape 'property) value))

;; Properties pro inspector-window

(defmethod properties ((p point))
  '(x y r phi))

(defmethod properties ((c circle))
  '(color thickness filledp radius))

(defmethod properties ((p polygon))
  '(color thickness filledp closedp))

(defmethod properties ((w window))
  '(delegate shape background))
