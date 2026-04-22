-- Upgrade_V2_AddProductDetails.sql
ALTER TABLE tblProducts
ADD Description NVARCHAR(500) NULL,
	ImageUrl NVARCHAR(200) NULL,
	Stock INT NOT NULL DEFAULT 0;

-- Update seed data
UPDATE tblProducts SET
	Description = 'Candy is a sweet treat enjoyed by people of all ages around the world. It comes in a wide variety of flavors, colors, and textures, ranging from chewy gummies to hard, crunchy sweets. Often made with sugar, flavorings, and sometimes chocolate or fruit extracts, candy is commonly associated with celebrations, holidays, and special moments. Its bright appearance and delightful taste make it a favorite indulgence for satisfying a sweet craving.',
	Stock = 10
WHERE Name = 'Candy'

UPDATE tblProducts SET
	Description = 'Dark chocolate is a rich and indulgent treat known for its deep, intense flavor and smooth texture. Made with a higher percentage of cocoa and less sugar than regular chocolate, it often has a slightly bitter taste that many people enjoy. Dark chocolate is also appreciated for its potential health benefits, as it contains antioxidants that may support heart health. Its bold taste and luxurious feel make it a popular choice for desserts, snacks, and gourmet recipes.',
	Stock = 8
WHERE Name = 'Dark Chocolate'

UPDATE tblProducts SET
	Description = 'Milk chocolate is a smooth and creamy treat that is loved for its sweet and mild flavor. Made with cocoa, milk, and sugar, it has a softer taste compared to dark chocolate, making it especially popular among those who prefer a less intense chocolate experience. Its velvety texture and comforting sweetness make it perfect for snacking, desserts, and a wide variety of confectionery creations enjoyed around the world.',
	Stock = 5
WHERE Name = 'Milk Chocolate'

UPDATE tblProducts SET
	Description = 'White chocolate is a sweet and creamy confection known for its smooth texture and delicate flavor. Unlike dark and milk chocolate, it is made from cocoa butter, sugar, and milk, without cocoa solids, which gives it its pale ivory color and mild taste. Its rich, buttery sweetness makes it a popular choice for desserts, baked goods, and decorative treats, adding a touch of elegance and indulgence to any recipe.',
	Stock = 0
WHERE Name = 'White Chocolate'