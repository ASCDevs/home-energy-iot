<template>
    <transition name="fade">
        <router-view />
    </transition>
</template>

<style>
    html, body {
        height: 100%;
    }

    .fade-enter-active, .fade-leave-active {
        transition: opacity 0.5s ease;
    }

    .fade-enter-from, .fade-leave-to {
        opacity: 0;
    }
</style>