using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public CliApp(
        IUserRepository userRepository,
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

 public async Task StartAsync()
{
    bool running = true;

    while (running)
    {
        Console.WriteLine();
        Console.WriteLine("1. Create user");
        Console.WriteLine("2. Create post");
        Console.WriteLine("3. Add comment");
        Console.WriteLine("4. View posts");
        Console.WriteLine("5. View specific post");
        Console.WriteLine("0. Exit");

        string? choice = Console.ReadLine();

        switch (choice)
        {
           case "1":
    await CreateUserAsync();
    break;

            case "2":
    await CreatePostAsync();
    break;

            case "3":
    await AddCommentAsync();
    break;

            case "4":
    ViewPosts();
    break;

            case "5":
    await ViewSpecificPostAsync();
    break;

            case "0":
                running = false;
                break;

            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }
}

private async Task CreateUserAsync()
{
    Console.Write("Username: ");
    string? username = Console.ReadLine();

    Console.Write("Password: ");
    string? password = Console.ReadLine();

    User user = new User
    {
        Username = username ?? "",
        Password = password ?? ""
    };

    User createdUser = await userRepository.AddAsync(user);

    Console.WriteLine($"User created with ID: {createdUser.Id}");
}

private async Task CreatePostAsync()
{
    Console.Write("Title: ");
    string? title = Console.ReadLine();

    Console.Write("Body: ");
    string? body = Console.ReadLine();

    Console.Write("User ID: ");
    int userId = int.Parse(Console.ReadLine() ?? "0");

    Post post = new Post
    {
        Title = title ?? "",
        Body = body ?? "",
        UserId = userId
    };

    Post createdPost = await postRepository.AddAsync(post);

    Console.WriteLine($"Post created with ID: {createdPost.Id}");
}
private async Task AddCommentAsync()
{
    Console.Write("Body: ");
    string? body = Console.ReadLine();

    Console.Write("User ID: ");
    int userId = int.Parse(Console.ReadLine() ?? "0");

    Console.Write("Post ID: ");
    int postId = int.Parse(Console.ReadLine() ?? "0");

    Comment comment = new Comment
    {
        Body = body ?? "",
        UserId = userId,
        PostId = postId
    };

    Comment createdComment = await commentRepository.AddAsync(comment);

    Console.WriteLine($"Comment created with ID: {createdComment.Id}");
}
private void ViewPosts()
{
    IQueryable<Post> posts = postRepository.GetManyAsync();

    foreach (Post post in posts)
    {
        Console.WriteLine($"ID: {post.Id} - Title: {post.Title}");
    }
}
private async Task ViewSpecificPostAsync()
{
    Console.Write("Post ID: ");
    int postId = int.Parse(Console.ReadLine() ?? "0");

    Post post = await postRepository.GetSingleAsync(postId);

    Console.WriteLine();
    Console.WriteLine($"Title: {post.Title}");
    Console.WriteLine($"Body: {post.Body}");
    Console.WriteLine("Comments:");

    IQueryable<Comment> comments = commentRepository.GetManyAsync();

    foreach (Comment comment in comments.Where(c => c.PostId == postId))
    {
        Console.WriteLine($"- {comment.Body}");
    }
}
}
