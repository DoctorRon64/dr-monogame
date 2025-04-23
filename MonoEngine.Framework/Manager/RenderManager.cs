using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Framework;

namespace MonoEngine.Framework;

public class RenderManager : BaseSingleton<RenderManager> {
    private readonly List<Entity> renderableEntities = new();
    public RenderManager() => Console.WriteLine("Initializing RenderSystem");

    public void Register(Entity entity) => renderableEntities.Add(entity);
    public void Unregister(Entity entity) => renderableEntities.Remove(entity);
    public IReadOnlyList<Entity> GetAllEntities() => renderableEntities;    
    
    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
        foreach (Entity entity in renderableEntities) {
            SpriteComponent sprite = entity.GetComponent<SpriteComponent>();
            Transform transform = entity.GetComponent<Transform>();

            Texture2D texture2D = sprite.GetTexture();
            if (texture2D == null) continue;
        
            spriteBatch.Draw(texture2D, transform.Position, null, sprite.GetColor(),
                transform.Rotation, transform.Origin, transform.Scale, sprite.GetSpriteEffects(), 0);
        }

        spriteBatch.End();
    }
}