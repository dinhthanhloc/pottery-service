-- =========================================
-- 1. Categories
-- =========================================
INSERT INTO categories (name, description, created_at, updated_at)
VALUES
    ('Am tra', 'Danh muc am tra gom su', NOW(), NOW()),
    ('Bat', 'Danh muc bat gom su', NOW(), NOW()),
    ('Dia', 'Danh muc dia gom su', NOW(), NOW()),
    ('Coc', 'Danh muc coc gom su', NOW(), NOW());

-- =========================================
-- 2. Products
-- =========================================
INSERT INTO products (category_id, name, sku, description, current_price, is_active, created_at, updated_at)
VALUES
    ((SELECT id FROM categories WHERE name = 'Am tra'), 'Am tra men lam', 'AM-001', 'Am tra gom su men lam', 350000, TRUE, NOW(), NOW()),
    ((SELECT id FROM categories WHERE name = 'Am tra'), 'Am tra hoa tiet sen', 'AM-002', 'Am tra hoa tiet sen', 420000, TRUE, NOW(), NOW()),
    ((SELECT id FROM categories WHERE name = 'Bat'), 'Bat com men trang', 'BAT-001', 'Bat com gom su men trang', 45000, TRUE, NOW(), NOW()),
    ((SELECT id FROM categories WHERE name = 'Bat'), 'Bat canh hoa nau', 'BAT-002', 'Bat canh gom su hoa nau', 55000, TRUE, NOW(), NOW()),
    ((SELECT id FROM categories WHERE name = 'Dia'), 'Dia tron men xanh', 'DIA-001', 'Dia tron gom su men xanh', 80000, TRUE, NOW(), NOW()),
    ((SELECT id FROM categories WHERE name = 'Coc'), 'Coc uong tra co quai', 'COC-001', 'Coc gom su co quai', 65000, TRUE, NOW(), NOW());

-- =========================================
-- 3. Product price histories
-- =========================================
INSERT INTO product_price_histories (product_id, price, valid_from, note)
VALUES
    ((SELECT id FROM products WHERE sku = 'AM-001'), 320000, NOW() - INTERVAL '30 days', 'Gia cu'),
    ((SELECT id FROM products WHERE sku = 'AM-001'), 350000, NOW() - INTERVAL '10 days', 'Cap nhat gia moi'),

    ((SELECT id FROM products WHERE sku = 'AM-002'), 400000, NOW() - INTERVAL '20 days', 'Gia ban dau'),
    ((SELECT id FROM products WHERE sku = 'AM-002'), 420000, NOW() - INTERVAL '5 days', 'Tang gia'),

    ((SELECT id FROM products WHERE sku = 'BAT-001'), 40000, NOW() - INTERVAL '25 days', 'Gia cu'),
    ((SELECT id FROM products WHERE sku = 'BAT-001'), 45000, NOW() - INTERVAL '7 days', 'Gia hien tai'),

    ((SELECT id FROM products WHERE sku = 'BAT-002'), 55000, NOW() - INTERVAL '15 days', 'Gia ban dau'),

    ((SELECT id FROM products WHERE sku = 'DIA-001'), 75000, NOW() - INTERVAL '12 days', 'Gia cu'),
    ((SELECT id FROM products WHERE sku = 'DIA-001'), 80000, NOW() - INTERVAL '3 days', 'Gia hien tai'),

    ((SELECT id FROM products WHERE sku = 'COC-001'), 60000, NOW() - INTERVAL '18 days', 'Gia cu'),
    ((SELECT id FROM products WHERE sku = 'COC-001'), 65000, NOW() - INTERVAL '2 days', 'Gia hien tai');

-- =========================================
-- 4. Sales
-- =========================================
INSERT INTO sales (sale_code, sale_date, total_amount, customer_name, note, created_at)
VALUES
    ('SALE-0001', NOW() - INTERVAL '3 days', 790000, 'Nguyen Van A', 'Don hang 1', NOW()),
    ('SALE-0002', NOW() - INTERVAL '2 days', 245000, 'Tran Thi B', 'Don hang 2', NOW()),
    ('SALE-0003', NOW() - INTERVAL '1 day', 535000, 'Le Van C', 'Don hang 3', NOW()),
    ('SALE-0004', NOW(), 220000, 'Pham Thi D', 'Don hang 4', NOW());

-- =========================================
-- 5. Sale items
-- =========================================
INSERT INTO sale_items (sale_id, product_id, quantity, unit_price, line_total)
VALUES
    -- SALE-0001 = 350000 + 2*45000 + 350000 = 790000
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0001'), (SELECT id FROM products WHERE sku = 'AM-001'), 1, 350000, 350000),
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0001'), (SELECT id FROM products WHERE sku = 'BAT-001'), 2, 45000, 90000),
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0001'), (SELECT id FROM products WHERE sku = 'AM-002'), 1, 350000, 350000),

    -- SALE-0002 = 3*55000 + 80000 = 245000
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0002'), (SELECT id FROM products WHERE sku = 'BAT-002'), 3, 55000, 165000),
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0002'), (SELECT id FROM products WHERE sku = 'DIA-001'), 1, 80000, 80000),

    -- SALE-0003 = 5*65000 + 2*45000 + 120000 = 535000
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0003'), (SELECT id FROM products WHERE sku = 'COC-001'), 5, 65000, 325000),
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0003'), (SELECT id FROM products WHERE sku = 'BAT-001'), 2, 45000, 90000),
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0003'), (SELECT id FROM products WHERE sku = 'DIA-001'), 1, 120000, 120000),

    -- SALE-0004 = 2*80000 + 1*60000 = 220000
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0004'), (SELECT id FROM products WHERE sku = 'DIA-001'), 2, 80000, 160000),
    ((SELECT id FROM sales WHERE sale_code = 'SALE-0004'), (SELECT id FROM products WHERE sku = 'COC-001'), 1, 60000, 60000);
