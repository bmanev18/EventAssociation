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
  - [ ] S1
  - [ ] S2
  - [ ] S3
  - [ ] F1 - Description is too long
  - [ ] F2 - Event is cancelled
  - [ ] F3 - Event is active
    
- Use Case 4: The creator updates the start and end time of the event
  - [ ] S1
  - [ ] S2
  - [ ] S3
  - [ ] S4
  - [ ] S5
  - [ ] F1 - Start date is after end date
  - [ ] F2 - Start time is after end date
  - [ ] F3 - Event duration too short
  - [ ] F4 - Event duration too short
  - [ ] F5 - Event start time too early
  - [ ] F6 - Event end time after 01:00
  - [ ] F7 - Event is active
  - [ ] F8 - Event is cancelled
  - [ ] F9 - Event duration too long
  - [ ] F10 - Event start time is in the past
  - [ ] F11 - Event durations spans invalid time.
        
- Use Case 5: The creator makes the event public
  - [ ] S1
  - [ ] F2 - Event is cancelled
    
- Use Case 6: The creator makes the event private
  - [ ] S1
  - [ ] S2
  - [ ] F1 - Event is active.
  - [ ] F2 - Event is cancelled
      
- Use Case 7: The creator sets maximum number of guests
  - [ ] S1
  - [ ] S2
  - [ ] S3
  - [ ] F1 - Event is active
  - [ ] F2 - Event is cancelled
  - [ ] F3 - Gust number higher than location's max number
  - [ ] F4 - Guest number too small
  - [ ] F5 - Guest number exceeds maximum
      
- Use Case 8: The creator readies an event
  - [ ] S1
  - [ ] F1 - Event is draft
  - [ ] F2 - event is cancelled
  - [ ] F3 - Event is in the past
  - [ ] F4 - Event title is default
  
- Use Case 9: The creator activates an event
  - [ ] S1
  - [ ] S2
  - [ ] S3
  - [ ] F1 - Event is draft
  - [ ] F2 - Event is cancelled
      
- Use Case 10: An anonymous user (aka Anon) registers a new Guest accoun
  - [ ] S1
  - [ ] F1 – Email domain incorrect
  - [ ] F2 – Email format incorrect
  - [ ] F3 – First name is invalid
  - [ ] F4 – Last name is invalid
  - [ ] F5 – Email already used
  - [ ] F6 – Names contain non-letter characters
  - [ ] F7 – URL invalid format
    
- Use Case 11: Guest participates public even
  - [ ] S1
  - [ ] F1 – Event not active
  - [ ] F2 – No more room
  - [ ] F3 – Cannot join active event
  - [ ] F4 – Cannot join private event
  - [ ] F5 – Guest is already participating
    
- Use Case 12: Guest cancels event participation
  - [ ] S1
  - [ ] S2
  - [ ] F1 - Event is active

- Use Case 13: Guest is invited to even
  - [ ] S1
  - [ ] F1 – Event is draft or cancelled
  - [ ] F2 – No more room
  - [ ] F3 – Guest is already invited
  - [ ] F4 – Guest is already participating
  
- Use Case 14: Guest accepts invitation
  - [ ] S1
  - [ ] F1 – Invitation not found
  - [ ] F2 – Too many guests
  - [ ] F3 - Event is cancelled
  - [ ] F4 – Event is ready

- Use Case 15: Guest declines invitation
  - [ ] S1
  - [ ] S2
  - [ ] F1 - Invitation not found
  - [ ] F2 - Event is cancelled
