# Clean Code impairs your ability to manage interconnected state #

This is a platformer study, which I used to learn how to manage the code that has a lot of interconnected state.
It's an attempt to learn how it differs from other applications, like a strategy game (still a game, but a lot more subsystems loosely connected) or non-gamedev world (web app server and client code).
Below is a list of takeaways I learned (they happen to be anti-Clean-Code).
I describe it here for anyone interested, including myself from the future.

Note:
I found coming back again and again to the [Player Controller class](https://github.com/NoelFB/Celeste/blob/master/Source/Player/Player.cs) of [Celeste](https://www.celestegame.com/) thinking `oh, this is awful code, so 'unclean', I'll make it better`.
In the end, I ended up having somewhat more procedural and bare-bone code than they have (I don't have separate Update methods for States in FSM for example).
If you find information in this `Readme` useful you should check out Celeste code as well!

## Interconnected State ##

The takeaways revolve around managing highly interconnected state.
If you want to have a better idea what I am mean by that see the section below called <strong>How much interconnectedness</strong>.

## 1. Extracting code makes it more complex ##

When updating state variables, avoid extracting code into methods (or properties or classes or whatever).
You have to deal with a lot of complexity, by tuning your brain to alternate paths of executions based on the state variables.
You want the code that updates them be clearly visible.
By hiding away the code that updates the state, you make it more difficult to stay on top of the possible execution paths.
It is fine to extract pieces of code that just read the state (in order to create something)

This is fine:
```c#
private static float GravityScaler(float jumpHeight, float timeToPeak)
{
    var newGravity = (-2 * jumpHeight) / (timeToPeak * timeToPeak);
    return newGravity / Physics.gravity.y;
}

// ...

private void Update()
{
    // ...
    ascendingGravityScale =
        GravityScaler(stanceSettings.jumpHeight, stanceSettings.jumpTimeToPeak);
    // ...
            
}
```

This is not fine:
```c#
private void UpdateGravities()
{
    ascendingGravityScale =
        GravityScaler(stanceSettings.jumpHeight, stanceSettings.jumpTimeToPeak);
    fallingGravityScale = ascendingGravityScale * stanceSettings.jumpFallingGravityMultiplier;
    jumpButtonReleasedGravityScale =
        ascendingGravityScale * stanceSettings.jumpVariableHeightGravityMultiplier;
}

private void FinishJump()
{
    canJumpDuringThisFlight = true;
    jumpButtonReleased = false;
    jumpTimer = 0.0f;
    isJumping = false;
    _rigidbody2D.gravityScale = ascendingGravityScale;
    _state = UpdateState(State.Default);
}

private void UpdateDash() {
    if (_state == State.Dashing)
    {
        dashTimer -= Time.fixedDeltaTime;
        if (dashTimer > 0)
        {
            return;
        }

        // end of dash
        _trailRenderer.emitting = false;
        if (isGrounded)
        {
            _rigidbody2D.gravityScale = ascendingGravityScale;
            _state = UpdateState(State.Default);
        }
        else
        {
            // in air dash
            _rigidbody2D.gravityScale = fallingGravityScale;
            _state = UpdateState(State.InAir);
        }
    }
}

private void Update() {
    UpdateGravities();
    if (isGrounded && !wasGrounded) {
        FinishJump();
    }
    UpdateDash();
    // ...
}
```

You can see the issue in the if statement with `isGrounded` and `wasGrounded`. 
All of the state variables are modified 'somewhere'.
In the example it's a tiny bit of `Update()` logic.
Consider this example containing full `Update()` code.
Have fun looking for them scattered around everywhere, and reasoning about them as a whole system.

(It took me a lot of back and forth extraction and inlining to actually realize this and resist the urge to extract every piece of Update code to methods!)

## 2. Accept Big Ordered Methods ##

If you learned to code somewhat recently, or at least you're somewhat familiar with Clean Code you may tremble when you see
`Update()` and `FixedUpdate()` in the `PlayerController` class. It's fine. 
For me, as an author I have a fair understanding of it.
For you it's probably difficult to reason about such a huge method.
It may be the sole reason why teams prefer to split up the code into methods - members need to read and learn the code of others.
However, building upon the Takeaway 1, for the interconnected state applications it is important to lay out clearly, how the state can change.
Breaking up long methods into chunks does not simplify the complex state management that needs to happen.

Programs that have a lot of state, usually run in a loop.
Some prefer to hide this loop and expose events as the building blocks for code architecture instead.
You can see that this is the approach of the `InputSystem` in this game, sending events and reacting to them.
As long as there is no interconnected state, such event driven architecture is a breeze.
But the input events from the player need to modify the state, that is also being modified in the ordinary `Update()` loop anyway.
Now there is a conundrum, how do you deal with the order of the events?
What happens if you press both dash and jump at the same time?
What should have the priority?

If you where to keep the blocks separate you can still check whether the other action is already happening before beginning yours.
This however makes it difficult to reason about as a full system.
You would like to have a clear way of expressing that, eg. `dash has the priority over jumping or moving updates`.
Having this code in a big procedure like `Update()` let's you do just that.
You start with the cases you want handled first, and then proceed to the others.
If you modify the state variables that are shared between mechanics (eg. velocity) you clearly see it couple lines above.
You can wrap the updates into if statements, though crude, they make the code easy to follow.
As it is the logic of updating your player controller what's complex, not your code.

So, having all updates that need to be synced in one giant method helps with reasoning about the execution order.
On top of that, there is an easy way to stop the execution for this frame, and move on to the next one: `return;`.
You can immediately cut further updates to indicate there is no use of doing them (if player character is dashing, I don't want to go through checks whether he's jumping or moving, I already know all of the details of its movement for this frame.)
Having the same code split across set of methods corresponding to events would require another set of variables allowing them to be synchronized.
It would increase the number of `if` statements in the methods as well.
Having ability to `return;` from `Update()` lets me skip all the `if (!dashing)` in the remaining part.

## 3. Embrace comments ##

Learning Clean Code to the fullest, I considered comments my enemies. 
I was taught that I should instead extract code to methods and give them meaningful names instead of commenting.
Other advice says to keep comments that explain 'the why', which is not covered in the code logic itself ('the how' and 'the what').
However, building on Takeaways 1 and 2, having large methods with a lot of state changes, I found myself in need to document it sometimes.
I have comments like `// jump started` sprinkled all over the code.
It is true that they grow stale quicker than you think and they require you to update them while updating the code.
But they can be of tremendous help while coming back to the project after a week+ of break, and needing to relearn the logic of `Update()` method.

Religious abstinence from commenting code seems folly to me, after working on this project.
There are places where dropping a comment saves you tens of minutes of work and helps you when you try to maintain all the information about the state in your head.
Let me say it one more time: <strong>it is the logic what is complex here</strong>.
Just as the big updates and no code extraction, the comments are the deliberate way of helping myself keep in touch with all that is going on in the project.

## 4. It is possible to handle the complexity ##

This is a separate takeaway as well as a conclusion of the past three.
The project, especially the player controller class, is a complex one to build right, and remain knowledgeable about it for a long time.
The Clean Code techniques help with handling the complexity, and make a fantastic job in doing so, but if you use them religiously
you might fall into the following trap:
1. You start an ambitious and complex project with state management
2. You follow all the Clean Code rules and your project grows quite a bit
3. You no longer feel like you understand how it works, due to layers of layers of stuff you extracted

It got me and I needed to come back at the drawing board and mentally escape the urges to extract, cut methods in size, and so on.
My overall takeaway from this study, is that I can manage the complexity of the program that has a lot of state, and I shouldn't
be too eager with my extracting habits and method/class-size fever.
It's way more important to focus on the real task, and treat the Clean Code techniques as the tools, not as the rule set for every program.

## How much interconnectedness ##
Consider typical web development HTTP server-side code.
Such application can be broken down in two major parts:
- dealing with requests and building responses
- maintaining state, such as thread pools, connections to other services (databases, queues, etc.)

The logic of building an HTTP response does not depend on the number of active connections that are open to the database, as long as the code is able to fetch the needed data.
It also won't matter the server uptime, or whether it's using a brand new connection or an old one.
And vice-versa, closing connection does not need to know anything about particular incoming request.

Now consider the code for the platformer, especially the player controller part.
Game developers often use Finite State Machine to help themselves reduce the state management complexity.
Then, player character can only be in one of the states, and it makes it easier to reason about what should happen given specific events occur (like being hit by an enemy).
However, State Machine applies to states that are mutually exclusive, for example in this game you can't be both dashing and walking.
But you can be in air and move left or right.
Just as you can be in air, because you jumped, or because you fell from a platform.
Or you can be hit when being invulnerable, after taking a hit a second ago.
Being invulnerable can't be a dedicated state, because you can be walking or dashing while being invulnerable.
This means that even though State Machine helps, it does not make the state management complexity go away.

In order to understand this interconnectedness a little bit better consider jumping in this game.
The jump code uses three different gravity scales in order to make the jump [feel better](https://youtu.be/hG9SzQxaCm8?si=0kfxEVw0nE4N4s4u).
There is the default gravity, which is changed into falling gravity once the character reaches the peak of the jump.
Then there is also the third one, that kicks in when player releases the jump button before character reaches the peak of the jump (it is a component of a variable height jump technique).

Now, having this in mind, whenever the other parts of the code will interact with the in-air character they need to consider each phase of the jump.
The player controller code, needs to adapt the outcome of the solution based on the state of the jump.
For example, if the character is hit in-air, you need to consider three possibilities, because he can be in three stages of the jump.
The information about the stages of the jump needs to be easily accessible (state variables) and modifiable (can't be read-only).
The same goes for any other interaction that might happen during the jump (dashing, switching battle stance, etc.).
Which in turns leads to accessing and modifying state variables all the time!
Pretty much everything in the player controller modifies state and needs to be accounted for.


