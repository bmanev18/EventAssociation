# Completed Feature Status

- Base Classes.
  - [x] ValueObject Base Classes
  - [x] Entity Base Classes
  - [x] Aggregate Base Classes
        
- Use Case 1: Event creator creates new event.
  - [x] S1
  - [x] S2
  - [x] S3
  - [x] S4
      
- Use Case 2: The creator updates the title of an existing event
  - [x] S1
  - [x] S2
  - [x] F1 - Title is empty
  - [x] F2 - Title is too short
  - [x] F3 - Title is too long
  - [x] F4 - Title is empty
  - [x] F5 - Event is active
  - [x] F6 - event is cancelled
      
- Use Case 3: The creator updates the description of an event
  - [x] S1
  - [x] S2
  - [x] S3
  - [x] F1 - Description is too long
  - [x] F2 - Event is cancelled
  - [x] F3 - Event is active
    
- Use Case 4: The creator updates the start and end time of the event
  - [x] S1
  - [x] S2
  - [x] S3
  - [x] S4
  - [x] S5
  - [x] F1 - Start date is after end date
  - [x] F2 - Start time is after end date
  - [x] F3 - Event duration too short
  - [x] F4 - Event duration too short
  - [x] F5 - Event start time too early
  - [x] F6 - Event end time after 01:00
  - [x] F7 - Event is active
  - [x] F8 - Event is cancelled
  - [x] F9 - Event duration too long
  - [x] F10 - Event start time is in the past
  - [x] F11 - Event durations spans invalid time.
        
- Use Case 5: The creator makes the event public
  - [x] S1
  - [x] F2 - Event is cancelled
    
- Use Case 6: The creator makes the event private
  - [x] S1
  - [x] S2
  - [x] F1 - Event is active.
  - [x] F2 - Event is cancelled
      
- Use Case 7: The creator sets maximum number of guests
  - [x] S1
  - [x] S2
  - [x] S3
  - [x] F1 - Event is active
  - [x] F2 - Event is cancelled
  - [x] F3 - Gust number higher than location's max number
  - [x] F4 - Guest number too small
  - [x] F5 - Guest number exceeds maximum
      
- Use Case 8: The creator readies an event
  - [x] S1
  - [x] F1 - Event is draft
  - [x] F2 - event is cancelled
  - [x] F3 - Event is in the past
  - [x] F4 - Event title is default
  
- Use Case 9: The creator activates an event
  - [x] S1
  - [x] S2
  - [x] S3
  - [x] F1 - Event is draft
  - [x] F2 - Event is cancelled
      
- Use Case 10: An anonymous user (aka Anon) registers a new Guest account
  - [x] S1
  - [x] F1 – Email domain incorrect
  - [x] F2 – Email format incorrect
  - [x] F3 – First name is invalid
  - [x] F4 – Last name is invalid
  - [x] F5 – Email already used
  - [x] F6 – Names contain non-letter characters
  - [x] F7 – URL invalid format
    
- Use Case 11: Guest participates public event **//<!-TODO-Waiting for EFC-->**
  - [x] S1
  - [x] F1 – Event not active
  - [x] F2 – No more room
  - [x] F3 – Cannot join active event
  - [x] F4 – Cannot join private event
  - [x] F5 – Guest is already participating
    
- Use Case 12: Guest cancels event participation  **//<!-TODO-Waiting for EFC-->** 
  - [x] S1
  - [x] S2
  - [x] F1 - Event is active

- Use Case 13: Guest is invited to event 
  - [x] S1
  - [x] F1 – Event is draft or cancelled
  - [x] F2 – No more room
  - [x] F3 – Guest is already invited
  - [x] F4 – Guest is already participating
  
- Use Case 14: Guest accepts invitation
  - [x] S1
  - [ ] F1 – Invitation not found (Not in domain)
  - [x] F2 – Too many guests
  - [x] F3 - Event is cancelled
  - [x] F4 – Event is ready

- Use Case 15: Guest declines invitation
  - [x] S1
  - [x] S2
  - [ ] F1 - Invitation not found (Not in domain)
  - [x] F2 - Event is cancelled
