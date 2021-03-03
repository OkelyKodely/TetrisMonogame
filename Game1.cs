using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class Game1 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    KeyboardState _currentKeyboardState;
    KeyboardState _previousKeyboardState;
    PieceManager pieceManager;
    System.Collections.Generic.List<object> pieceTuple;
    Piece piece;
    System.Collections.Generic.List<Piece> pieces;
    System.Collections.Generic.List<Block> blocks;
    private int row = -4;
    private int column = 5;
    float timeSinceLastDown = 0f;
    float timeSinceLastColorChangeBg = 0f;
    int level = 1;
    int clear = 0;
    float speed = 0.0f;
        
    public Game1()
        : base()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 500;
        graphics.PreferredBackBufferHeight = 850;
        Content.RootDirectory = "Content";
        pieces = new System.Collections.Generic.List<Piece>();
        this.blocks = new System.Collections.Generic.List<Block>();
        pieceManager = new PieceManager();
        pieceManager.setGame(this);
        this.Window.Title = "Level: " + this.level + " Line: " + this.clear;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        //piece = new Piece(this);
        this.pieceTuple = pieceManager.createNewPieceBlocks(this.piece, this.row, this.column);
        this.piece = (Piece)this.pieceTuple[0];
        //this.pieces = (System.Collections.Generic.List<Piece>)this.pieceTuple[1];
        this.pieces.Add(piece);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        timeSinceLastColorChangeBg += (float)gameTime.ElapsedGameTime.TotalSeconds;
        Color color = Color.Blue;
        if (timeSinceLastColorChangeBg > 0.1f)
        {
            System.Random random = new System.Random();
            int val = random.Next(10);
            if (val == 0)
                color = Color.Blue;
            else if (val == 1)
                color = Color.Yellow;
            else if (val == 2)
                color = Color.Red;
            else if (val == 3)
                color = Color.Orange;
            else if (val == 4)
                color = Color.Green;
            else if (val == 5)
                color = Color.Purple;
            else if (val == 6)
                color = Color.YellowGreen;
            else if (val == 7)
                color = Color.Gray;
            else if (val == 8)
                color = Color.White;
            else if (val == 9)
                color = Color.Beige;
            color = Color.DodgerBlue;
            timeSinceLastColorChangeBg = 0f;
            GraphicsDevice.Clear(color);
        }
        
        // Before handling input
        _currentKeyboardState = Keyboard.GetState();

        // TODO: Add your update logic here
        if ((_currentKeyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down)) ||
            (_currentKeyboardState.IsKeyDown(Keys.Left) && _previousKeyboardState.IsKeyUp(Keys.Left)) ||
            (_currentKeyboardState.IsKeyDown(Keys.Right) && _previousKeyboardState.IsKeyUp(Keys.Right)) ||
            (_currentKeyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up)))
        {
            if (_currentKeyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (!pieceIsStacked() && !(this.piece.block1.Row >= 16 ||
                    this.piece.block2.Row >= 16 ||
                    this.piece.block3.Row >= 16 ||
                    this.piece.block4.Row >= 16))
                {
                    down();
                }
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Left) && _previousKeyboardState.IsKeyUp(Keys.Left))
            {
                if (!pieceIsJuxtaposedToTheLeft())
                {
                    left();
                }
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Right) && _previousKeyboardState.IsKeyUp(Keys.Right))
            {
                if (!pieceIsJuxtaposedToTheRight())
                {
                    right();
                }
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
            {
                flip();
            }
        }

        // After handling input
        _previousKeyboardState = Keyboard.GetState();

        timeSinceLastDown += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(piece.block1.Row == 0 ||
            piece.block2.Row == 0 ||
            piece.block3.Row == 0 ||
            piece.block4.Row == 0) {
                if (pieceIsStacked()) {
                    Exit();
                }
        }

        if (timeSinceLastDown > 2.3f - this.speed && (this.piece.block1.Row >= 16 ||
            this.piece.block2.Row >= 16 ||
            this.piece.block3.Row >= 16 ||
            this.piece.block4.Row >= 16 ||
            pieceIsStacked())) {
            this.row = -4;
            this.column = 5;
            if (piece.block1 != null)
                this.blocks.Add(piece.block1);
            if (piece.block2 != null)
                this.blocks.Add(piece.block2);
            if (piece.block3 != null)    
                this.blocks.Add(piece.block3);
            if (piece.block4 != null)    
                this.blocks.Add(piece.block4);
            clearLinesWhenNeeded();
            this.pieceTuple = pieceManager.createNewPieceBlocks(this.piece, this.row, this.column);
            this.piece = (Piece)this.pieceTuple[0];
            this.pieces.Add(this.piece);
        }

        if (timeSinceLastDown > 2.3f - this.speed)
        {
            down();
            timeSinceLastDown = 0f;
        }

        base.Update(gameTime);
    }

    public void clearLinesWhenNeeded()
    {
        System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();

        int count = 0;

        for (int j = 0; j < 17; j++)
        {
            for (int i = 0; i < this.blocks.Count; i++)
            {
                    if (this.blocks[i].Row == j)
                    {
                        count++;
                    }
                    if (count == 10)
                    {
                        list.Add(j);
                        break;
                    }
            }
            count = 0;
        }
        if (list.Count > 0)
        {
            for (int j = 0; j < list.Count; j++)
            {
                for (int i = 0; i < this.blocks.Count; i++)
                {
                        if (this.blocks[i].Row == list[j])
                        {
                            this.blocks[i].Row = -2000;
                        }
                }
                for (int i = 0; i < this.blocks.Count; i++)
                {
                        if (this.blocks[i].Row < list[j])
                        {
                            this.blocks[i].Row += 1;
                        }
                }
            }
        }
        this.clear += list.Count;
        if (this.clear / 5 >= 1 && this.clear != 0) {
            this.level = this.clear / 5 + 1;
            this.speed = (0.10f * (this.level - 1));
            if (this.level < 4)
                this.speed = 1.8f + (0.0333f * (this.level - 1));
            else
                this.speed = 2.0f;
            this.Window.Title = "Level: " + this.level + " Line: " + this.clear;
        } else {
            this.Window.Title = "Level: " + this.level + " Line: " + this.clear;
        }

    }

    protected void flip()
    {
        if (this.piece.kind.Equals("line"))
        {
            if(this.piece.position.Equals("up"))
            {
                this.row += 3;
                this.column += 3;
                this.piece.block1.Row += 3;
                this.piece.block1.Column += 3;
                this.piece.block2.Row += 2;
                this.piece.block2.Column += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("right");
            }
            else if (this.piece.position.Equals("right"))
            {
                this.row += 3;
                this.column -= 3;
                this.piece.block1.Row += 3;
                this.piece.block1.Column -= 3;
                this.piece.block2.Row += 2;
                this.piece.block2.Column -= 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("down");
            }
            else if (this.piece.position.Equals("down"))
            {
                this.row -= 3;
                this.column -= 3;
                this.piece.block1.Row -= 3;
                this.piece.block1.Column -= 3;
                this.piece.block2.Row -= 2;
                this.piece.block2.Column -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("left");
            }
            else if (this.piece.position.Equals("left"))
            {
                this.row -= 3;
                this.column += 3;
                this.piece.block1.Row -= 3;
                this.piece.block1.Column += 3;
                this.piece.block2.Row -= 2;
                this.piece.block2.Column += 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("up");
            }
        }
        else if (this.piece.kind.Equals("rs"))
        {
            if (this.piece.position.Equals("up"))
            {
                this.row += 1;
                this.column += 3;
                this.piece.block1.Row += 1;
                this.piece.block1.Column += 3;
                this.piece.block2.Column += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("right");
            }
            else if (this.piece.position.Equals("right"))
            {
                this.row += 3;
                this.column -= 1;
                this.piece.block1.Row += 3;
                this.piece.block1.Column -= 1;
                this.piece.block2.Row += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("down");
            }
            else if (this.piece.position.Equals("down"))
            {
                this.row -= 1;
                this.column -= 3;
                this.piece.block1.Row -= 1;
                this.piece.block1.Column -= 3;
                this.piece.block2.Column -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("left");
            }
            else if (this.piece.position.Equals("left"))
            {
                this.row -= 3;
                this.column += 1;
                this.piece.block1.Row -= 3;
                this.piece.block1.Column += 1;
                this.piece.block2.Row -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("up");
            }
        }
        else if (this.piece.kind.Equals("la"))
        {
            if (this.piece.position.Equals("up"))
            {
                this.row += 3;
                this.column += 1;
                this.piece.block1.Row += 3;
                this.piece.block1.Column += 1;
                this.piece.block2.Row += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("right");
            }
            else if (this.piece.position.Equals("right"))
            {
                this.row += 1;
                this.column -= 3;
                this.piece.block1.Row += 1;
                this.piece.block1.Column -= 3;
                this.piece.block2.Column -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("down");
            }
            else if (this.piece.position.Equals("down"))
            {
                this.row -= 3;
                this.column -= 1;
                this.piece.block1.Row -= 3;
                this.piece.block1.Column -= 1;
                this.piece.block2.Row -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("left");
            }
            else if (this.piece.position.Equals("left"))
            {
                this.row -= 1;
                this.column += 3;
                this.piece.block1.Row -= 1;
                this.piece.block1.Column += 3;
                this.piece.block2.Column += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("up");
            }
        }
        else if (this.piece.kind.Equals("ra"))
        {
            if (this.piece.position.Equals("up"))
            {
                this.row += 1;
                this.column += 3;
                this.piece.block1.Row += 1;
                this.piece.block1.Column += 3;
                this.piece.block2.Column += 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("right");
            }
            else if (this.piece.position.Equals("right"))
            {
                this.row += 3;
                this.column -= 1;
                this.piece.block1.Row += 3;
                this.piece.block1.Column -= 1;
                this.piece.block2.Row += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("down");
            }
            else if (this.piece.position.Equals("down"))
            {
                this.row -= 1;
                this.column -= 3;
                this.piece.block1.Row -= 1;
                this.piece.block1.Column -= 3;
                this.piece.block2.Column -= 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("left");
            }
            else if (this.piece.position.Equals("left"))
            {
                this.row -= 3;
                this.column += 1;
                this.piece.block1.Row -= 3;
                this.piece.block1.Column += 1;
                this.piece.block2.Row -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("up");
            }
        }
        else if (this.piece.kind.Equals("ls"))
        {
            if (this.piece.position.Equals("up"))
            {
                this.row += 3;
                this.column += 1;
                this.piece.block1.Row += 3;
                this.piece.block1.Column += 1;
                this.piece.block2.Row += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("right");
            }
            else if (this.piece.position.Equals("right"))
            {
                this.row += 1;
                this.column -= 3;
                this.piece.block1.Row += 1;
                this.piece.block1.Column -= 3;
                this.piece.block2.Column -= 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("down");
            }
            else if (this.piece.position.Equals("down"))
            {
                this.row -= 3;
                this.column -= 1;
                this.piece.block1.Row -= 3;
                this.piece.block1.Column -= 1;
                this.piece.block2.Row -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column -= 1;
                this.piece.setPosition("left");
            }
            else if (this.piece.position.Equals("left"))
            {
                this.row -= 1;
                this.column += 3;
                this.piece.block1.Row -= 1;
                this.piece.block1.Column += 3;
                this.piece.block2.Column += 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column += 1;
                this.piece.setPosition("up");
            }
        }
        else if (this.piece.kind.Equals("hat"))
        {
            if (this.piece.position.Equals("up"))
            {
                this.piece.block2.Row -= 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column -= 1;
                this.piece.block4.Column -= 2;
                this.piece.setPosition("right");
            }
            else if (this.piece.position.Equals("right"))
            {
                this.piece.block2.Column += 2;
                this.piece.block3.Row -= 1;
                this.piece.block3.Column += 1;
                this.piece.block4.Row -= 2;
                this.piece.setPosition("down");
            }
            else if (this.piece.position.Equals("down"))
            {
                this.piece.block2.Row += 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column += 1;
                this.piece.block4.Column += 2;
                this.piece.setPosition("left");
            }
            else if (this.piece.position.Equals("left"))
            {
                this.piece.block2.Column -= 2;
                this.piece.block3.Row += 1;
                this.piece.block3.Column -= 1;
                this.piece.block4.Row += 2;
                this.piece.setPosition("up");
            }
        }
    }

    private bool pieceIsJuxtaposedToTheLeft()
    {
        for (int i = 0; i < this.pieces.Count; i++)
        {
            Piece p = this.pieces[i];
            if (p == this.piece)
                continue;
            if ((p.block1.Row == this.piece.block1.Row && p.block1.Column + 1 == this.piece.block1.Column) ||
                (p.block1.Row == this.piece.block2.Row && p.block1.Column + 1 == this.piece.block2.Column) ||
                (p.block1.Row == this.piece.block3.Row && p.block1.Column + 1 == this.piece.block3.Column) ||
                (p.block1.Row == this.piece.block4.Row && p.block1.Column + 1 == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block2.Row == this.piece.block1.Row && p.block2.Column + 1 == this.piece.block1.Column) ||
                (p.block2.Row == this.piece.block2.Row && p.block2.Column + 1 == this.piece.block2.Column) ||
                (p.block2.Row == this.piece.block3.Row && p.block2.Column + 1 == this.piece.block3.Column) ||
                (p.block2.Row == this.piece.block4.Row && p.block2.Column + 1 == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block3.Row == this.piece.block1.Row && p.block3.Column + 1 == this.piece.block1.Column) ||
                (p.block3.Row == this.piece.block2.Row && p.block3.Column + 1 == this.piece.block2.Column) ||
                (p.block3.Row == this.piece.block3.Row && p.block3.Column + 1 == this.piece.block3.Column) ||
                (p.block3.Row == this.piece.block4.Row && p.block3.Column + 1 == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block4.Row == this.piece.block1.Row && p.block4.Column + 1 == this.piece.block1.Column) ||
                (p.block4.Row == this.piece.block2.Row && p.block4.Column + 1 == this.piece.block2.Column) ||
                (p.block4.Row == this.piece.block3.Row && p.block4.Column + 1 == this.piece.block3.Column) ||
                (p.block4.Row == this.piece.block4.Row && p.block4.Column + 1 == this.piece.block4.Column))
            {
                return true;
            }
        }

        return false;
    }

    private bool pieceIsJuxtaposedToTheRight()
    {
        for (int i = 0; i < this.pieces.Count; i++)
        {
            Piece p = this.pieces[i];
            if (p == this.piece)
                continue;
            if ((p.block1.Row == this.piece.block1.Row && p.block1.Column - 1 == this.piece.block1.Column) ||
                (p.block1.Row == this.piece.block2.Row && p.block1.Column - 1 == this.piece.block2.Column) ||
                (p.block1.Row == this.piece.block3.Row && p.block1.Column - 1 == this.piece.block3.Column) ||
                (p.block1.Row == this.piece.block4.Row && p.block1.Column - 1 == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block2.Row == this.piece.block1.Row && p.block2.Column - 1 == this.piece.block1.Column) ||
                (p.block2.Row == this.piece.block2.Row && p.block2.Column - 1 == this.piece.block2.Column) ||
                (p.block2.Row == this.piece.block3.Row && p.block2.Column - 1 == this.piece.block3.Column) ||
                (p.block2.Row == this.piece.block4.Row && p.block2.Column - 1 == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block3.Row == this.piece.block1.Row && p.block3.Column - 1 == this.piece.block1.Column) ||
                (p.block3.Row == this.piece.block2.Row && p.block3.Column - 1 == this.piece.block2.Column) ||
                (p.block3.Row == this.piece.block3.Row && p.block3.Column - 1 == this.piece.block3.Column) ||
                (p.block3.Row == this.piece.block4.Row && p.block3.Column - 1 == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block4.Row == this.piece.block1.Row && p.block4.Column - 1 == this.piece.block1.Column) ||
                (p.block4.Row == this.piece.block2.Row && p.block4.Column - 1 == this.piece.block2.Column) ||
                (p.block4.Row == this.piece.block3.Row && p.block4.Column - 1 == this.piece.block3.Column) ||
                (p.block4.Row == this.piece.block4.Row && p.block4.Column - 1 == this.piece.block4.Column))
            {
                return true;
            }
        }

        return false;
    }

    private bool pieceIsStacked()
    {
        for (int i = 0; i < this.pieces.Count; i++)
        {
            Piece p = this.pieces[i];
            if (p == this.piece)
                continue;
            if((p.block1.Row == this.piece.block1.Row + 1 && p.block1.Column == this.piece.block1.Column) ||
                (p.block1.Row == this.piece.block2.Row + 1 && p.block1.Column == this.piece.block2.Column) ||
                (p.block1.Row == this.piece.block3.Row + 1 && p.block1.Column == this.piece.block3.Column) ||
                (p.block1.Row == this.piece.block4.Row + 1 && p.block1.Column == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block2.Row == this.piece.block1.Row + 1 && p.block2.Column == this.piece.block1.Column) ||
                (p.block2.Row == this.piece.block2.Row + 1 && p.block2.Column == this.piece.block2.Column) ||
                (p.block2.Row == this.piece.block3.Row + 1 && p.block2.Column == this.piece.block3.Column) ||
                (p.block2.Row == this.piece.block4.Row + 1 && p.block2.Column == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block3.Row == this.piece.block1.Row + 1 && p.block3.Column == this.piece.block1.Column) ||
                (p.block3.Row == this.piece.block2.Row + 1 && p.block3.Column == this.piece.block2.Column) ||
                (p.block3.Row == this.piece.block3.Row + 1 && p.block3.Column == this.piece.block3.Column) ||
                (p.block3.Row == this.piece.block4.Row + 1 && p.block3.Column == this.piece.block4.Column))
            {
                return true;
            }
            else if ((p.block4.Row == this.piece.block1.Row + 1 && p.block4.Column == this.piece.block1.Column) ||
                (p.block4.Row == this.piece.block2.Row + 1 && p.block4.Column == this.piece.block2.Column) ||
                (p.block4.Row == this.piece.block3.Row + 1 && p.block4.Column == this.piece.block3.Column) ||
                (p.block4.Row == this.piece.block4.Row + 1 && p.block4.Column == this.piece.block4.Column))
            {
                return true;
            }
        }
            
        return false;

    }

    public void updatePieceLocation()
    {
        int rowDifference = this.row - piece.block1.Row;
        int columnDifference = this.column - piece.block1.Column;
        piece.block1.Row += rowDifference;
        piece.block1.Column += columnDifference;
        piece.block2.Row += rowDifference;
        piece.block2.Column += columnDifference;
        piece.block3.Row += rowDifference;
        piece.block3.Column += columnDifference;
        piece.block4.Row += rowDifference;
        piece.block4.Column += columnDifference;
    }
        
    public void down()
    {
        pushPieceDown();
        updatePieceLocation();
    }

    public void left()
    {
        if (piece.block1.Column > 0 && piece.block2.Column > 0 && piece.block3.Column > 0 && piece.block4.Column > 0) {
            pushPieceLeft();
            updatePieceLocation();
        }
    }

    public void right()
    {
        if (piece.block1.Column < 9 && piece.block2.Column < 9 && piece.block3.Column < 9 && piece.block4.Column < 9) {
            pushPieceRight();
            updatePieceLocation();
        }
    }

    public void pushPieceDown()
    {
        this.row += 1;
    }

    public void pushPieceLeft()
    {
        this.column -= 1;
    }

    public void pushPieceRight()
    {
        this.column += 1;
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here
        spriteBatch.Begin();
        drawPiece();
        spriteBatch.End();

        base.Draw(gameTime);
    }

    protected void drawPiece()
    {
        for (int i = 0; i < this.pieces.Count; i++)
        {
            Piece piece = this.pieces[i];
            spriteBatch.Draw(piece.block1.texture, new Rectangle(piece.block1.Column * 50, piece.block1.Row * 50, 50, 50), Color.White);
            spriteBatch.Draw(piece.block2.texture, new Rectangle(piece.block2.Column * 50, piece.block2.Row * 50, 50, 50), Color.White);
            spriteBatch.Draw(piece.block3.texture, new Rectangle(piece.block3.Column * 50, piece.block3.Row * 50, 50, 50), Color.White);
            spriteBatch.Draw(piece.block4.texture, new Rectangle(piece.block4.Column * 50, piece.block4.Row * 50, 50, 50), Color.White);
        }
    }
}