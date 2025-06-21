class Portfolio {
    constructor() {
        this.init();
        this.bindEvents();
        this.setupIntersectionObserver();
    }

    init() {
        this.navToggle = document.querySelector('.nav-toggle');
        this.navMenu = document.querySelector('.nav-menu');
        this.scrollToTopBtn = document.getElementById('scroll-to-top');
        this.contactForm = document.querySelector('.contact-form');
        this.filterBtns = document.querySelectorAll('.filter-btn');
        this.projectCards = document.querySelectorAll('.project-card');
        
        // Create mobile nav overlay
        this.createMobileNavOverlay();
        
        // Initialize project filters
        this.initProjectFilters();
        
        // Initialize form validation
        this.initFormValidation();
        
        // Initialize scroll effects
        this.initScrollEffects();
    }

    bindEvents() {
        // Navigation toggle
        this.navToggle?.addEventListener('click', () => this.toggleMobileNav());
        
        // Close mobile nav when clicking overlay
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('nav-overlay')) {
                this.closeMobileNav();
            }
        });

        // Smooth scroll for navigation links
        document.querySelectorAll('a[href^="#"]').forEach(link => {
            link.addEventListener('click', (e) => this.handleSmoothScroll(e));
        });

        // Scroll to top button
        this.scrollToTopBtn?.addEventListener('click', () => this.scrollToTop());

        // Window scroll events
        window.addEventListener('scroll', () => {
            this.handleScroll();
            this.updateActiveNavLink();
        }, { passive: true });

        // Window resize events
        window.addEventListener('resize', () => this.handleResize(), { passive: true });

        // Form submission
        this.contactForm?.addEventListener('submit', (e) => this.handleFormSubmit(e));

        // Project filter buttons
        this.filterBtns.forEach(btn => {
            btn.addEventListener('click', (e) => this.filterProjects(e));
        });

        // Keyboard navigation
        document.addEventListener('keydown', (e) => this.handleKeyboardNav(e));
    }

    createMobileNavOverlay() {
        const overlay = document.createElement('div');
        overlay.className = 'nav-overlay';
        overlay.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            z-index: 999;
            opacity: 0;
            visibility: hidden;
            transition: all 0.3s ease;
        `;
        document.body.appendChild(overlay);
        this.navOverlay = overlay;
    }

    toggleMobileNav() {
        const isOpen = this.navMenu.classList.contains('show-nav');
        
        if (isOpen) {
            this.closeMobileNav();
        } else {
            this.openMobileNav();
        }
    }

    openMobileNav() {
        this.navMenu.classList.add('show-nav');
        this.navOverlay.style.opacity = '1';
        this.navOverlay.style.visibility = 'visible';
        document.body.style.overflow = 'hidden';
        
        // Update toggle button aria-label
        this.navToggle.setAttribute('aria-label', 'Close navigation menu');
        this.navToggle.setAttribute('aria-expanded', 'true');
    }

    closeMobileNav() {
        this.navMenu.classList.remove('show-nav');
        this.navOverlay.style.opacity = '0';
        this.navOverlay.style.visibility = 'hidden';
        document.body.style.overflow = '';
        
        // Update toggle button aria-label
        this.navToggle.setAttribute('aria-label', 'Open navigation menu');
        this.navToggle.setAttribute('aria-expanded', 'false');
    }

    handleSmoothScroll(e) {
        e.preventDefault();
        const targetId = e.target.getAttribute('href');
        const targetElement = document.querySelector(targetId);
        
        if (targetElement) {
            const offsetTop = targetElement.offsetTop - 80; // Account for fixed header
            
            window.scrollTo({
                top: offsetTop,
                behavior: 'smooth'
            });
            
            // Close mobile nav if open
            if (this.navMenu.classList.contains('show-nav')) {
                this.closeMobileNav();
            }
        }
    }

    scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }

    handleScroll() {
        const scrollY = window.scrollY;
        
        // Show/hide scroll to top button
        if (scrollY > 300) {
            this.scrollToTopBtn?.classList.add('visible');
        } else {
            this.scrollToTopBtn?.classList.remove('visible');
        }

        // Add shadow to header when scrolling
        const header = document.querySelector('header');
        if (scrollY > 0) {
            header?.classList.add('scrolled');
        } else {
            header?.classList.remove('scrolled');
        }
    }

    updateActiveNavLink() {
        const sections = document.querySelectorAll('section[id]');
        const navLinks = document.querySelectorAll('.nav-menu a[href^="#"]');
        
        let currentSection = '';
        
        sections.forEach(section => {
            const sectionTop = section.offsetTop - 100;
            const sectionHeight = section.offsetHeight;
            
            if (window.scrollY >= sectionTop && window.scrollY < sectionTop + sectionHeight) {
                currentSection = section.getAttribute('id');
            }
        });
        
        navLinks.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href') === `#${currentSection}`) {
                link.classList.add('active');
            }
        });
    }

    handleResize() {
        // Close mobile nav on resize to desktop
        if (window.innerWidth >= 768 && this.navMenu.classList.contains('show-nav')) {
            this.closeMobileNav();
        }
    }

    handleKeyboardNav(e) {
        // Close mobile nav with Escape key
        if (e.key === 'Escape' && this.navMenu.classList.contains('show-nav')) {
            this.closeMobileNav();
        }
    }

    initProjectFilters() {
        // Set initial active filter
        this.filterBtns[0]?.classList.add('active');
    }

    filterProjects(e) {
        const category = e.target.getAttribute('data-category');
        
        // Update active filter button
        this.filterBtns.forEach(btn => btn.classList.remove('active'));
        e.target.classList.add('active');
        
        // Filter project cards with animation
        this.projectCards.forEach(card => {
            const cardCategory = card.getAttribute('data-category');
            const shouldShow = category === 'all' || category === cardCategory;
            
            if (shouldShow) {
                card.style.display = 'block';
                setTimeout(() => {
                    card.style.opacity = '1';
                    card.style.transform = 'translateY(0)';
                }, 50);
            } else {
                card.style.opacity = '0';
                card.style.transform = 'translateY(20px)';
                setTimeout(() => {
                    card.style.display = 'none';
                }, 300);
            }
        });
    }

    initFormValidation() {
        const nameInput = this.contactForm?.querySelector('#name');
        const emailInput = this.contactForm?.querySelector('#email');
        const messageInput = this.contactForm?.querySelector('#message');
        
        if (!nameInput || !emailInput || !messageInput) return;
        
        // Real-time validation
        [nameInput, emailInput, messageInput].forEach(input => {
            input.addEventListener('blur', () => this.validateField(input));
            input.addEventListener('input', () => this.clearFieldError(input));
        });
    }

    validateField(field) {
        const value = field.value.trim();
        const fieldName = field.getAttribute('name');
        let isValid = true;
        let errorMessage = '';
        
        // Reset field styles
        field.classList.remove('error');
        
        switch (fieldName) {
            case 'name':
                if (!value) {
                    errorMessage = 'Name is required';
                    isValid = false;
                } else if (value.length < 2) {
                    errorMessage = 'Name must be at least 2 characters';
                    isValid = false;
                }
                break;
                
            case 'email':
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!value) {
                    errorMessage = 'Email is required';
                    isValid = false;
                } else if (!emailRegex.test(value)) {
                    errorMessage = 'Please enter a valid email address';
                    isValid = false;
                }
                break;
                
            case 'message':
                if (!value) {
                    errorMessage = 'Message is required';
                    isValid = false;
                } else if (value.length < 10) {
                    errorMessage = 'Message must be at least 10 characters';
                    isValid = false;
                }
                break;
        }
        
        if (!isValid) {
            this.showFieldError(field, errorMessage);
        }
        
        return isValid;
    }

    showFieldError(field, message) {
        field.classList.add('error');
        const errorElement = field.parentNode.querySelector('.error-message');
        if (errorElement) {
            errorElement.textContent = message;
        }
    }

    clearFieldError(field) {
        field.classList.remove('error');
        const errorElement = field.parentNode.querySelector('.error-message');
        if (errorElement) {
            errorElement.textContent = '';
        }
    }

    async handleFormSubmit(e) {
        e.preventDefault();
        
        const nameInput = this.contactForm.querySelector('#name');
        const emailInput = this.contactForm.querySelector('#email');
        const messageInput = this.contactForm.querySelector('#message');
        const submitBtn = this.contactForm.querySelector('.submit-btn');
        
        // Validate all fields
        const isNameValid = this.validateField(nameInput);
        const isEmailValid = this.validateField(emailInput);
        const isMessageValid = this.validateField(messageInput);
        
        if (!isNameValid || !isEmailValid || !isMessageValid) {
            return;
        }
        
        // Show loading state
        submitBtn.classList.add('loading');
        submitBtn.disabled = true;
        
        try {
            // Simulate form submission
            await this.simulateFormSubmission({
                name: nameInput.value,
                email: emailInput.value,
                message: messageInput.value
            });
            
            // Show success message
            this.showFormSuccess();
            
            // Reset form
            this.contactForm.reset();
            
        } catch (error) {
            console.error('Form submission error:', error);
            this.showFormError('Something went wrong. Please try again.');
        } finally {
            // Remove loading state
            submitBtn.classList.remove('loading');
            submitBtn.disabled = false;
        }
    }

    async simulateFormSubmission(data) {
        // Simulate API call
        return new Promise((resolve) => {
            setTimeout(() => {
                console.log('Form submitted:', data);
                resolve();
            }, 2000);
        });
    }

    showFormSuccess() {
        const message = this.createNotification('Message sent successfully!', 'success');
        document.body.appendChild(message);
    }

    showFormError(errorMessage) {
        const message = this.createNotification(errorMessage, 'error');
        document.body.appendChild(message);
    }

    createNotification(text, type) {
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.textContent = text;
        notification.style.cssText = `
            position: fixed;
            top: 2rem;
            right: 2rem;
            padding: 1rem 1.5rem;
            border-radius: 0.5rem;
            color: white;
            font-weight: 500;
            z-index: 10000;
            transform: translateX(100%);
            transition: transform 0.3s ease;
            ${type === 'success' ? 'background: #10b981;' : 'background: #ef4444;'}
        `;
        
        // Animate in
        setTimeout(() => {
            notification.style.transform = 'translateX(0)';
        }, 100);
        
        // Auto remove
        setTimeout(() => {
            notification.style.transform = 'translateX(100%)';
            setTimeout(() => {
                notification.remove();
            }, 300);
        }, 3000);
        
        return notification;
    }

    setupIntersectionObserver() {
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('animate-fade-in-up');
                }
            });
        }, observerOptions);
        
        // Observe sections and cards
        document.querySelectorAll('section, .project-card, .skill-category').forEach(el => {
            observer.observe(el);
        });
    }

    initScrollEffects() {
        // Parallax effect for hero section
        const heroSection = document.querySelector('.hero-section');
        if (heroSection) {
            window.addEventListener('scroll', () => {
                const scrolled = window.pageYOffset;
                const rate = scrolled * -0.5;
                heroSection.style.transform = `translateY(${rate}px)`;
            }, { passive: true });
        }
    }
}

// Initialize portfolio when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new Portfolio();
});

// Add CSS for header scroll effect
const style = document.createElement('style');
style.textContent = `
    header.scrolled {
        box-shadow: var(--shadow-md);
        background: rgba(255, 255, 255, 0.98);
    }
    
    .nav-menu a.active {
        color: var(--primary-color);
    }
    
    .notification {
        box-shadow: var(--shadow-lg);
    }
    
    .project-card {
        opacity: 1;
        transform: translateY(0);
        transition: all 0.3s ease;
    }
`;
document.head.appendChild(style);