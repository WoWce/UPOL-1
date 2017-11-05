(defclass mouse-window (window)
  ((curr-position :initform nil)))




(defmethod window-mouse-move ((w mouse-window) button position)
  (let ((shape (find-clicked-shape w position)))
    (if shape
            (mouse-enter shape button position))
      (if (slot-value w 'curr-position)
          (if (not (eql (find-clicked-shape (slot-value w 'curr-position)) nil))
              (if (eql (find-clicked-shape w (slot-value w 'curr-position))
                       (find-clicked-shape w position))
                  (mouse-leave shape button position))))))
    (setf (slot-value w 'curr-position) position))





(defmethod mouse-enter ((shape shape) button position)
  ())

(defmethod mouse-leave ((shape shape) button position)
  (print "no"))

(defmethod install-mouse-move-callback ((w mouse-window))
  (mg:set-callback 
   (mg-window w) 
   :mouse-move (lambda (mgw button x y)
		 (declare (ignore mgw))
		 (window-mouse-move
                  w
                  button 
                  (move (make-instance 'point) x y)))))

(defmethod install-callbacks ((w mouse-window))
  (call-next-method)
  (install-mouse-move-callback w)
  w)


#|
(setf w (make-instance 'mouse-window))
(setf c (make-instance 'circle))
(move (set-radius (set-thickness (set-color c :green) 20) 55)
      148
      100)
(set-filledp c t)
(set-shape w c)
|#