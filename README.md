## MRSTWeb_Project
SITE DE LOGISTICĂ PENTRU O COMPANIE DE ELECTROCASNICE. 

Uploaded base template 08-03

Works basically like this:
**register OR login --> go to main app page --> add box details (weight, dimensions, additional instructions [this way up, fragile, handle with care, keep dry, keep upright, perishable, do not stack, flammable, explosive, do not open before date]) --> choose time period of delivery --> choose driver / truck (out of the list that are working during delivery time period) --> ** can check progress in `progress report` page

Pages: [16/15]
- [ ] index
- [X] main application (box fitting in trucks etc.)
- [ ] box details
- [X] ** choose driver (can be on sepparate page, where truck is shown based on user choice)
- [ ] look at box positions (shown all boxes in truck like this [https://www.optioryx.com/cartonization]) (maybe click and drag boxes manually, idk)
- [X] ** maybe click on box to show box details (instructions, weight, dimensions, etc.)
- [X] login
- [X] register
- [ ] user profile
- [ ] about / contacts
- [ ] report an issue (damaged goods, delay, missing items)
- [ ] progress report (delays, how long until path is completed)
- [X] driver timetable page (all drivers are visible here) (what drivers are available, what are their schedules, what driver should the box be issued to)
- [X] add driver to db (driver details, truck details)
- [ ] notifications ?
- [X] admin page  

Page layout:
Index:

        A welcome message.
        Brief introduction to the app's purpose (e.g., "Efficient and secure box delivery").
        CTA buttons like "Login," "Register," or "Get Started."
        User testimonials or feature highlights to encourage user engagement.
        Links to the About/Contacts and FAQ sections for assistance.

Main Application (Box Fitting in Trucks, etc.):

        Visual representation of available trucks with box spaces and dimensions.
        Option to view available trucks based on weight, volume, or type of box.
        If applicable, a 3D interactive view of how boxes will fit inside trucks (this could be integrated with AI or an algorithm that suggests the most efficient packing method).

Box Details Page:

        Weight, dimensions, and volume of the box.
        Special instructions (e.g., "fragile," "handle with care," "keep dry").
        Add photos or images of the box (optional but helpful for special handling).
        Option to save the box info for future deliveries.

Choose Driver:

        Display available drivers and trucks for the selected time period.
        Show information about each driver: name, rating, vehicle type, and availability.
        Option to filter drivers based on user preferences (e.g., vehicle type, experience, distance from the box).
        Driver Profile Page: Clicking on a driver can take the user to their profile, showing detailed information like the truck type, license details, and reviews.

Look at Box Positions:

    Truck Space Layout:
        Visual representation of available truck space and where the boxes can be placed.
        The user can drag and drop the boxes into the truck space or the system can automatically optimize box placement.
        Show dimensions and weight distribution as users place boxes.
        Option to "zoom" in on specific boxes for more details (dimensions, instructions).

Click on Box to Show Details:

        Show the box’s information (weight, dimensions, special handling instructions, etc.).
        Option to edit the box details or delete it.
        A button to view the delivery status or progress report for that box.

Login Page:

        A form with fields for username/email and password.
        "Forgot Password" link for password recovery.
        Option to log in via Google, Facebook, or other OAuth services for convenience.
        
Register Page:

        Fields for creating an account: username, email, password, and phone number.
        Option to sign up using social media or email.
        Terms and Conditions checkbox to ensure the user agrees to policies before registration.

About / Contacts:

        A brief introduction about the app and its purpose.
        The benefits of using the service (e.g., cost efficiency, secure handling of items, real-time tracking).
        A mission statement or company values.
        Contact details (email, phone, support hours).
        A form for users to submit inquiries or feedback.
        Links to social media or help documentation (FAQs).

Report an Issue Page:

        Provide a dropdown to categorize the issue (e.g., "Damage to goods," "Delay," "Missing items," etc.).
        Text field for a detailed description of the issue.
        Option to upload photos or documentation.
        A submit button that sends the report to customer support.

Progress Report Page:

        [?] A visual representation of the delivery path (e.g., a map with the driver’s route and current location).
        Real-time updates on the status of the delivery (e.g., "In Transit," "Arriving Soon," "Delivered").
        A timeline showing key milestones (e.g., when the driver picked up the box, when they left the depot, etc.).
        A "Delays" section showing any changes to the expected delivery time.
        Option to contact the driver directly or customer support for updates.

Driver Timetable Page:

        A calendar or timetable that shows which drivers are available at specific times.
        Users can filter drivers based on their availability or location.
        An option for users to reserve a driver at a specific time slot.
        Display driver ratings, so users can make an informed decision about who to choose.

Add Driver to Database Page:

        Fields for adding a new driver to the database (name, contact details, truck type, experience level).
        Fields for truck details (make, model, capacity, license plate number).
        A photo upload option for driver’s identification and truck registration details.
        A checkbox to mark the driver as "active" or "inactive," depending on their availability.


### Main app usecases

    @startuml
    package "Index Page" {
    
      actor User
      actor Admin
      note right of User : Add basic description of what app does
    
      package "Content Management" {
        usecase "Main Application" as UC_Main
        usecase "Details/Active Deliveries/Progress Report" as UC_Details
        usecase "About Us/Contacts" as UC_Contacts
      }
    
      package "User Interaction" {
        usecase "Login" as UC_Login
        usecase "Register" as UC_Register
        usecase "User Profile" as UC_Profile
        usecase "Contact Support" as UC_Support
      }
      
    
      package "Admin Commands" {
        usecase "Admin Page" as UC_Admin
        usecase "Add new driver to database" as UC_AddDriver
        usecase "Driver timetable" as UC_DriverTimetable
      }
      
      User --> UC_Main
      User --> UC_Contacts
      User --> UC_Login
      UC_Login ..> UC_Profile
      UC_Profile ..> UC_Details
      note right of UC_Support : Greyed out `Report Issue with delivery` button until logged in
      User --> UC_Register
      User --> UC_Support
      User ----|> Admin
      Admin --> UC_Admin
      Admin --> UC_AddDriver
      Admin --> UC_DriverTimetable
     
    }
    @enduml

### Main app workflow

            @startuml
    start
        :Display Index Page;
        if (User logs into account?) then (Yes)
           :Access to Main application;
           :Box details is displayed;
           if (Save box details as template?) then (Yes)
              :Save as template;
           endif
           :Choose Truck / Driver;
           if (Chosen driver not available?) then (Unavailable)
              :Display driver availability and next available time;
           endif
           :Show positions of boxes in the truck;
           if (User wants to view box details dynamically?) then (Yes)
              :Display dynamic box details;
           endif
           :Confirm positions;
           :[?]Track truck and send delivery completion notification;
        else (No)
           :Back to Index;
        endif
    stop
    @enduml

`Display driver availability`, `Choose Truck/Driver`, `Box Details`, are separate pages

`Display box details`, `Notifications`,  can be new page/popup ?
