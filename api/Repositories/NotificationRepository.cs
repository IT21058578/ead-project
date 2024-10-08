
using api.Configurations;
using api.Models;

namespace api.Repositories
{

	/// <summary>
	/// The NotificationRepository class used to interact with the Notification Entity
	/// </summary>
	/// 
	/// <remarks>
	/// The NotificationRepository class is a repository class that inherits the MongoRepository 
	/// class and implements the MongoRepository<T> interface. It provides methods for
	/// interacting with the collection where Notifications are stored in the database.
	/// </remarks>
	public class NotificationRepository(AppDbContext dbContext) : MongoRepository<Notification>(dbContext)
	{
	}
}