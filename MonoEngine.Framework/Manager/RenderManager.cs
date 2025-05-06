using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Framework;
using MonoEngine.Framework.components;

namespace MonoEngine.Framework.Manager;

public class RenderManager : BaseSingleton<RenderManager> {
    private readonly List<Entity> renderableEntities = new();
    public RenderManager() => Console.WriteLine("Initializing RenderSystem");

    public void Register(Entity entity) => renderableEntities.Add(entity);
    public void Unregister(Entity entity) => renderableEntities.Remove(entity);
    public IReadOnlyList<Entity> GetAllEntities() => renderableEntities;

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        foreach (Entity entity in renderableEntities) {
            Sprite sprite = entity.GetComponent<Sprite>();
            Transform transform = entity.GetComponent<Transform>();

            if (sprite == null || transform == null) continue;
            
            Texture2D texture2D = sprite.Texture;
            if (texture2D == null) continue;

            spriteBatch.Draw(texture2D, transform.Position, null, sprite.Color,
                transform.Rotation, transform.Origin, transform.Scale, sprite.Effects, 0);
        }

        spriteBatch.End();
    }
}