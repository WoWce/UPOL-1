(defclass imitating-window (window)
  ((group :initform nil)))

(defmethod set-group ((w imitating-window) value)
  (setf (slot-value w 'group) value))

(defmethod group ((w imitating-window))
  (slot-value w 'group))

(defmethod add-to-group ((w imitating-window) w-group)
  (if (eql (group w) nil)
      (progn
        (set-group w (list w-group))
        (set-delegate w-group w))
    (progn
      (set-delegate w-group (last (group w)))
      (set-group w (append (group w) (list w-group)))
      (set-delegate w w-group))))

(defmethod change ((w imitating-window) message changed-obj args)
  (call-next-method)
  (if (not (eql (shape w) (shape (car (delegate w)))))
      (send-event w 'update-window changed-obj))
  (print args))

(defmethod update-window ((w imitating-window) sender changed-obj)
  (set-shape w (shape changed-obj)))

#|
(setf w (make-instance 'imitating-window))
(setf w2 (make-instance 'imitating-window))
(setf w3 (make-instance 'imitating-window))
(add-to-group w w2)
(add-to-group w w3)
(setf cr (make-instance 'circle))
(move (set-radius (set-thickness (set-color cr :darkslategrey) 20) 55)
      148
      100)
(set-shape w cr)
|#

      
  