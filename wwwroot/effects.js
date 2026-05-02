(function () {
    'use strict';

    const _state = {
        particleCanvas: null,
        particleAnimId: null,
        cursorGlowEl: null,
        cursorBound: false,
        rippleBound: false,
        typewriterTimeouts: [],
    };

    // ── 1. STARFIELD PARTICLES ──────────────────────────────────────────────
    function initParticles() {
        if (_state.particleAnimId) {
            cancelAnimationFrame(_state.particleAnimId);
            _state.particleAnimId = null;
        }
        if (_state.particleCanvas) {
            _state.particleCanvas.remove();
            _state.particleCanvas = null;
        }

        const canvas = document.createElement('canvas');
        canvas.id = 'sk-particles';
        canvas.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;z-index:-1;pointer-events:none;';
        document.body.appendChild(canvas);
        _state.particleCanvas = canvas;

        const ctx = canvas.getContext('2d');
        let W = canvas.width = window.innerWidth;
        let H = canvas.height = window.innerHeight;

        const COUNT = Math.min(120, Math.floor((W * H) / 12000));
        const COLORS = [
            'rgba(108, 92, 231,',
            'rgba(0, 245, 255,',
            'rgba(255, 0, 255,',
            'rgba(162, 155, 254,',
            'rgba(255, 255, 255,',
        ];

        function mkParticle() {
            return {
                x: Math.random() * W,
                y: Math.random() * H,
                vx: (Math.random() - 0.5) * 0.25,
                vy: (Math.random() - 0.5) * 0.25,
                size: Math.random() * 1.8 + 0.3,
                color: COLORS[Math.floor(Math.random() * COLORS.length)],
                alpha: Math.random() * 0.6 + 0.1,
                phase: Math.random() * Math.PI * 2,
            };
        }

        const particles = Array.from({ length: COUNT }, mkParticle);

        function draw(ts) {
            ctx.clearRect(0, 0, W, H);
            for (const p of particles) {
                p.phase += 0.018;
                const a = p.alpha * (0.5 + 0.5 * Math.sin(p.phase));

                const g = ctx.createRadialGradient(p.x, p.y, 0, p.x, p.y, p.size * 3);
                g.addColorStop(0, p.color + a + ')');
                g.addColorStop(0.5, p.color + (a * 0.3) + ')');
                g.addColorStop(1, p.color + '0)');

                ctx.beginPath();
                ctx.arc(p.x, p.y, p.size * 3, 0, Math.PI * 2);
                ctx.fillStyle = g;
                ctx.fill();

                ctx.beginPath();
                ctx.arc(p.x, p.y, p.size, 0, Math.PI * 2);
                ctx.fillStyle = p.color + a + ')';
                ctx.fill();

                p.x += p.vx;
                p.y += p.vy;
                if (p.x < -5) p.x = W + 5;
                if (p.x > W + 5) p.x = -5;
                if (p.y < -5) p.y = H + 5;
                if (p.y > H + 5) p.y = -5;
            }
            _state.particleAnimId = requestAnimationFrame(draw);
        }

        _state.particleAnimId = requestAnimationFrame(draw);

        window.addEventListener('resize', () => {
            W = canvas.width = window.innerWidth;
            H = canvas.height = window.innerHeight;
        }, { passive: true });
    }

    // ── 2. CURSOR GLOW ──────────────────────────────────────────────────────
    function initCursorGlow() {
        if (_state.cursorGlowEl) {
            _state.cursorGlowEl.remove();
        }

        const el = document.createElement('div');
        el.id = 'sk-cursor-glow';
        el.style.cssText = [
            'position:fixed', 'top:0', 'left:0',
            'width:500px', 'height:500px', 'border-radius:50%',
            'pointer-events:none', 'z-index:0',
            'background:radial-gradient(circle, rgba(108,92,231,0.07) 0%, rgba(0,245,255,0.04) 35%, transparent 70%)',
            'transition:opacity 0.3s ease', 'opacity:0', 'will-change:transform',
        ].join(';');
        document.body.appendChild(el);
        _state.cursorGlowEl = el;

        let mx = -1000, my = -1000, gx = -1000, gy = -1000, visible = false;

        function lerp(a, b, t) { return a + (b - a) * t; }

        function animate() {
            gx = lerp(gx, mx, 0.08);
            gy = lerp(gy, my, 0.08);
            el.style.transform = `translate(${gx - 250}px,${gy - 250}px)`;
            requestAnimationFrame(animate);
        }

        if (!_state.cursorBound) {
            document.addEventListener('mousemove', e => {
                mx = e.clientX; my = e.clientY;
                if (!visible) { el.style.opacity = '1'; visible = true; }
            }, { passive: true });
            document.addEventListener('mouseleave', () => {
                el.style.opacity = '0'; visible = false;
            }, { passive: true });
            _state.cursorBound = true;
        }

        requestAnimationFrame(animate);
    }

    // ── 3. BUTTON RIPPLE ────────────────────────────────────────────────────
    function initRipple() {
        if (_state.rippleBound) return;

        if (!document.getElementById('sk-ripple-style')) {
            const s = document.createElement('style');
            s.id = 'sk-ripple-style';
            s.textContent = '@keyframes skRipple{to{transform:scale(1);opacity:0}}';
            document.head.appendChild(s);
        }

        document.addEventListener('click', e => {
            const btn = e.target.closest('.sk-btn');
            if (!btn) return;
            const r = btn.getBoundingClientRect();
            const x = e.clientX - r.left;
            const y = e.clientY - r.top;
            const size = Math.max(r.width, r.height) * 2;
            const ripple = document.createElement('span');
            ripple.style.cssText = [
                'position:absolute',
                `left:${x - size / 2}px`,
                `top:${y - size / 2}px`,
                `width:${size}px`,
                `height:${size}px`,
                'border-radius:50%',
                'background:rgba(255,255,255,0.25)',
                'pointer-events:none',
                'transform:scale(0)',
                'animation:skRipple 0.55s cubic-bezier(0.4,0,0.2,1) forwards',
            ].join(';');
            btn.appendChild(ripple);
            ripple.addEventListener('animationend', () => ripple.remove(), { once: true });
        });

        _state.rippleBound = true;
    }

    // ── 4. 3D TILT ON STAT CARDS ────────────────────────────────────────────
    function initTilt() {
        document.querySelectorAll('.sk-stat-colored').forEach(card => {
            if (card._tiltBound) return;
            card._tiltBound = true;

            let tX = 0, tY = 0, cX = 0, cY = 0, rafId = null;
            function lerp(a, b, t) { return a + (b - a) * t; }

            card.addEventListener('mousemove', e => {
                const rc = card.getBoundingClientRect();
                tX = ((e.clientY - rc.top  - rc.height / 2) / (rc.height / 2)) * -10;
                tY = ((e.clientX - rc.left - rc.width  / 2) / (rc.width  / 2)) *  10;
                if (!rafId) {
                    function run() {
                        cX = lerp(cX, tX, 0.12);
                        cY = lerp(cY, tY, 0.12);
                        card.style.transform = `translateY(-6px) scale(1.03) perspective(800px) rotateX(${cX}deg) rotateY(${cY}deg)`;
                        rafId = (Math.abs(cX - tX) > 0.05 || Math.abs(cY - tY) > 0.05)
                            ? requestAnimationFrame(run)
                            : null;
                    }
                    rafId = requestAnimationFrame(run);
                }
            }, { passive: true });

            card.addEventListener('mouseleave', () => {
                if (rafId) { cancelAnimationFrame(rafId); rafId = null; }
                tX = tY = 0;
                card.style.transition = 'transform 0.5s cubic-bezier(0.34,1.56,0.64,1)';
                card.style.transform = '';
                setTimeout(() => { card.style.transition = ''; }, 500);
            }, { passive: true });
        });
    }

    // ── 5. COUNT-UP ANIMATION ───────────────────────────────────────────────
    function countUp(selector, duration) {
        duration = duration || 1200;
        document.querySelectorAll(selector || '[data-countup]').forEach(el => {
            const target = parseInt(el.dataset.countup, 10);
            if (isNaN(target)) return;
            const start = performance.now();
            function easeOut(t) { return 1 - Math.pow(2, -10 * t); }
            function tick(ts) {
                const p = Math.min((ts - start) / duration, 1);
                el.textContent = Math.round(target * easeOut(p)).toLocaleString('de-DE');
                if (p < 1) requestAnimationFrame(tick);
            }
            requestAnimationFrame(tick);
        });
    }

    // ── 6. TYPEWRITER EFFECT ────────────────────────────────────────────────
    function initTypewriter() {
        _state.typewriterTimeouts.forEach(clearTimeout);
        _state.typewriterTimeouts = [];

        const el = document.querySelector('.sk-hero-subtitle[data-typewriter]');
        if (!el) return;

        const text = el.dataset.typewriter;
        el.textContent = '';
        el.style.borderRight = '2px solid rgba(0,245,255,0.7)';
        el.style.paddingRight = '2px';

        const cursor = setInterval(() => {
            el.style.borderRightColor =
                el.style.borderRightColor === 'transparent'
                    ? 'rgba(0,245,255,0.7)'
                    : 'transparent';
        }, 530);

        let i = 0;
        function type() {
            if (i < text.length) {
                el.textContent += text.charAt(i++);
                const id = setTimeout(type, 28 + Math.random() * 22);
                _state.typewriterTimeouts.push(id);
            } else {
                const id = setTimeout(() => {
                    clearInterval(cursor);
                    el.style.borderRight = 'none';
                    el.style.paddingRight = '';
                }, 3000);
                _state.typewriterTimeouts.push(id);
            }
        }

        const startId = setTimeout(type, 400);
        _state.typewriterTimeouts.push(startId);
    }

    // ── PUBLIC API ──────────────────────────────────────────────────────────
    window.SK = {
        init() {
            initParticles();
            initCursorGlow();
            initRipple();
        },
        initTilt,
        initTypewriter,
        countUp,
    };

    // Auto-start
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => window.SK.init());
    } else {
        window.SK.init();
    }

})();
