# Theatre-Web-App
 A simple ticket reservation application for a theatre.
 
Features

General

- Unregistered users can look through a list of currently staged plays, with their own separate details pages, or browse a list of shows in the theatre’s program.
- Plays can be searched by a key word, whereas shows can be grouped by play, by key word, or by date.

Users

- Registered users can book tickets for available shows and browse a list of their previous reservations.
- If a user books seats but fails to confirm/pay, their reservation can be found in the “My Reservations” page where they can confirm or cancel it.
Note: a background service periodically (every 20 minutes) deletes unconfirmed tickets.
- Users can “print” their tickets – an action that returns a more detailed view of their reservation, including individual seat prices and rows.
 
Admin

- The app provides a default administrator (username: admin@theatre.com, password: admin).
- An admin can add and edit plays, and can also change their visibility from their details pages.
- An admin can add shows using existing plays and stages. Show date and time can be changed later.
- Currently seat pricing is set generally, for all past and future shows, using different seat categories. Administrators can manually change the category and its respective price for individual seats. Unfortunately this does not allow for pricing flexibility.

